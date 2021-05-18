# титульний аркуш 

# Introduction

Monitoring and observability for distributed systems include collecting, analyzing, and presenting various dynamic data regarding some particular processes running in the distributed system.
In a modern distributed systems environment, many works and practices are available for making these non-trivial tasks more manageable, even though monitoring remains one of the most complicated aspects to tackle while modeling production-ready software. The work exposes reasons behind such wide adoption of distributed systems for modern industry workloads, focusing on such practices as DevOps and SRE, making it so accessible for a broad audience.

The work also defines characteristics of distributed systems that make monitoring so complicated. These characteristics are used for understanding the methodology of monitoring and comparing related papers.

The main goal of the work is to examine the importance of monitoring and observability in the modern distributed system, taking into account the domain where applied. Also, to investigate collecting, analyzing, and presenting operational data in distributed systems for preventing critical situations, i.e., incidents. Besides technical challenges, the works also evaluate the monitoring from a product ownership point of view, describing the approaches to SLAs, monitoring culture mindset, incidents, et cetera. Also, incidents are discussed in work, focusing on recovering and learning from them, most importantly preventing them in the future. Then, the price of monitoring is discussed at various stages of the software product lifecycle, especially in production environments.

Also, the software product is built based on this study which shows the usage of modern monitoring best practices. All of the aspects are covered both conceptually and practically: metrics, alerts, logging, dashboards, messages, debugging, tracing. The software product is built for the reader's better understanding of the concepts described. Technologies used to create this sample include but are not limited to .NET Core, Prometheus, Docker, Docker-Compose, Kibana, Logstash, Zipkin, OpenTelemetry.

The information given in this thesis is based on the researcher's experience regarding building and architecting distributed systems and running and supporting such systems in production environments with a strong focus on reliability, stability, and safety.

The work is organized in following way: Chapter 1 describes related works and the motivation behind the research. Chapter 2 defines the domain of the work, going through distributed systems and their monitoring which evolved over time. Next on, in chapter 3 we focus on implementing monitoring by discussing both conceptual aspects and illustrating them with practical examples. Visualization of monitoring is presented in chapter 4. Finally, the work talks on preventive safety in distributed systems which could be achieved with multiple ways discussed in the chapter. Conclusions and future work are presented in chapter 5. 

# Related works

The field of distributed systems and monitoring is studied ever since parallel computations took place. [Cooper and Marzullo, 1991]. 
The terminology of monitoring and observability takes roots in system control theory and measures the level with regards to which the distributed system could be observed. In other words, how internal state of the system could be identified based on its outputs. The outputs were mostly the results of testing, either manual or automated. 

There is a huge amount of classical researches focused on creating own monitoring systems which were supposed to ensure stability and reliability of a distributed system [Mansouri-Samani, Masoud & Sloman, Morris, 1993]. These researches inspired us for creating this work as they were ones affecting how monitoring was built up back in the day when none of the modern approaches and framework existed.

Besides standard approaches to monitoring, it's critically important to predict the performance of the system.

Johng et al. [11] doing the research suggested two approaches to monitoring problem - benchmarking and simulation. These approaches turned out feasible, rational, fast, cheap and reliable. Following the same route, Lin et. al [14] suggested new way of exploring and identifying root causes for incidents in microservice architectures based on visualization of the data available (i.e., logs, metrics, traces). 

Talking about modern environments such as cloud, the observability of the system is defined the same way as back in the way, but the outputs of the system are different. The outputs are logs, metrics, traces, et cetera. They are generally used for defining the obversability, and, therefore, for identifying state of the system. 

There are also modern works which discuss approaches for monitoring distributed systems [Chauhan et al., 2013].
Based on that [Chauhan et al., 2013], Mostafa and Bonakdarpour [Mostafa and Bonakdarpour, 2015] were able to adapt these approaches for anomaly detection based on some properties. Therefore, anomalies could be recognized as incorrect behaviour of the system.
Out of modern academic world, there are also researches which disclose the aspects of monitoring which are highly applicable to today's state of technology. Moreover, they are already used by modern systems developers. A great example of such research is "On Observability and Monitoring of Distributed Systems – An Industry Interview Study" by Sina Niedermaier, Falko Koetter, Andreas Freymann, Stefan Wagner. That research was based on 28 interviews with monitoring and DevOps practitioners. In the end, it managed to identify key pain-points for interviewees and define possible future works for the industry, i.e., what could be the progress in monitoring distributed systems.

Preventive aspect of monitoring distributed systems was not address among top-notch researches, thus this work was created. It's supposed to fill that gap by carefully analyzing related works and discussing how monitoring could be preventive in the context of distributed systems which tend to fail.

## Distributed systems as domain field

The field of study is definitely distributed systems. Before we dive into approaches, terminology, instruments, it's important to outline the idea of modern distributed systems, taking into account the way they evolved with time since the term 'distributed system' is available in researches since early 80s. [Manzo et al., 1982].
Nevertheless, we could claim that the definition of distributed systems from 1980s is still applicable to modern systems. 

Distributed system contains of multiple processes which work on different servers and do the shared work. Obviously, that shared work is distributed among servers for more optimal scenario execution. These processes must communicate between each other for better integration via any underlying communication protocol (HTTP, AMQP, gRPC, et cetera) [Coulouris, 2011; Garg, 2002; Attiya and Welch, 2004].

Even though, systems were distributed by design since long time ago because multithreaded application also counts as distributed system even though the work is distributed between threads and cores, not physical servers. 

Over the last decade (2010-2020) distributed systems became a trend for developing high-load and data-intense systems. A nice remark to add here is that modern distributed systems are the most distributed the world has ever seen since they span multiple servers, cloud providers, countries, companies, and they execute variuous programs which base on different platforms and runtimes (such as Go, .NET, Java), but still are feasible to be ran. One would question why distributed systems are so relevant at all?
Distributed systems have a list of advantages over traditional centralized systems:
- lowered cost of infrastructure
- improved reliability and availability
- ease of extracting modules and composing them
- flexibility of configuration
- ability to grow step by step while improving system by replacing individual components

Even if each individual system is trivial and simple, integration of these components could create drastically huge complexity. By itself it creates multiple problems which did not exist in traditional systems which worked within one physical machine. In the end, the number of components in play increases statistical equity of error. 

These problems include but are not limited to:
- Multiple components can unexpectingly fail, possibly at the same time which leads to even more unexpected consequences due to other parts of the system react to invalid input from failed subsystems  
- Communication between subsystems can fail, including the transient network failures. In such environments, network quality and stability are extremely important.
- If network fails, the subsystems tend to retry requests / messages which could lead to fatally corrupted state of the system.
- Heterogeneity of the systems since they could be different on so many levels - the code which actually executes the logic, infrastructure technologies such as Docker images, virtual machines, or even serverless functions. Moreover, the teams could also be heterogenous. While such diverse environment could cultivate innovation, allowing multiple teams to pick the most appropriate technologies for their subsystems, at the same time it creates additional complexity for consistent approach for monitoring such systems.

## Monitoring distributed systems

So, looking after trends, and observing distributed computations and systems is an important problem since early ages of distributed systems [Cooper and Marzullo, 1991].
In 1987 Joyce [Joyce et al., 1987] identified five aspects which represent complexity of monitoring distributed systems. This could be considered as early attempt to identify key characteristics of problems that differentiate monitoring centralized systems from monitoring distributed systems

Here are these aspects:
- Distributed systems have much more points of touch where it's possible to control them. In some situations, that much more so it's seems unrealistic to cover them.
- Presense of non-deterministic delays in communication between subsystems since that makes impossible to make deterministic claims about the state of the system at given moment of time.
- Inherited non-determinism from distributed and asynchronous systems.
- Monitoring distributed systems changes their behaviour similarly to observer effect [4].
- The complexity of interaction between system and systems developers.

Considering the challenge of availability of distributed systems and their popularity, we must mention all instruments which became available with the technical progress.
One of fundamental instruments is Docker which allows to isolate the software product in the environment which, in its turn, ensures determinism while deploying Docker image (as programming artifact) into another environment.

Similarly, we must mention ever-growing popularity of cloud solutions which largely making the start easier for software products. By itself, cloud solutions are distributed systems in essence since solutions are created by combining multiple components. If before early 2010s cloud solutions were to inappropriate because of the pricing, now they are so cheap so it's feasible to use cloud instead maintaining own data-centers.
Nevertheless, growing popularity of cloud solutions, big data, orchestrators made us reconsider modelling monitoring for distributed systems because distributed systems themselves changed a lot. The main change was to split the system into much smaller, more concrete subsystems which obviously lead to increasing the number of components involved. Such change has direct impact on what to monitor because of the increased number of components. In the end, these components create extremely huge amounts of data as resulting monitoring artifact to be investigated.

Discussing the need of monitoring, it's important to mention that both system developers and managers want to have as much as possible data for decision making and control over distributed system for making the most out of it. So, such systems should be observed at each stage of their lifecycle - from prototyping to everyday support of production systems. As the area of monitoring changes, the monitoring chnanges as well by introducing new instruments, approaches, et cetera. The goals remain the same - to understands where the issues are, what are their root causes, timely alert the developers, identify incident, et cetera.

# Monitoring and its components

There are multiple problems monitoring could address which are described in full version of the work. In this chapter we will describe three main stages of which monitoring consists - data collection, processing and presentation.
Data collection:
Relevant data from the system must be composed into understadable format for the monitoring system. For instance, Prometheus collects data from subsystem and represents them via API endpoint in following format:

```
# HELP process_private_memory_bytes Process private memory size
# TYPE process_private_memory_bytes gauge
process_private_memory_bytes 74604544
# HELP process_virtual_memory_bytes Virtual memory size in bytes.
# TYPE process_virtual_memory_bytes gauge
process_virtual_memory_bytes 2223070081024
# HELP process_start_time_seconds Start time of the process since unix epoch in seconds.
# TYPE process_start_time_seconds gauge
process_start_time_seconds 1576244939.1073897
# HELP dotnet_total_memory_bytes Total known allocated memory
# TYPE dotnet_total_memory_bytes gauge
dotnet_total_memory_bytes 3013928
# HELP process_cpu_seconds_total Total user and system CPU time spent in seconds.
# TYPE process_cpu_seconds_total counter
process_cpu_seconds_total 0.796875
# HELP dotnet_collection_count_total GC collection count
# TYPE dotnet_collection_count_total counter
dotnet_collection_count_total{generation="1"} 0
dotnet_collection_count_total{generation="2"} 0
dotnet_collection_count_total{generation="0"} 0
# HELP process_working_set_bytes Process working set
# TYPE process_working_set_bytes gauge
process_working_set_bytes 56242176
# HELP process_num_threads Total number of threads
# TYPE process_num_threads gauge
process_num_threads 35
# HELP process_open_handles Number of open handles
# TYPE process_open_handles gauge
process_open_handles 566
```
FTODO: 

That data already corresponds to the format, thus could be collected by Prometheus scrapper.
Also, important thing to mention that data collection should not take up too much of the system resources.

Processing:
In some cases, there could be more data than enough, but that data does not hold much of informational value. For instance, there is no need to collect the data each second from some subsystems regarding their CPU load. In such cases, adaptive sampling could be applied. This would reduce the amount of data which is being stored, but, at the same time, it will not be critical for understanding the state of the system. In other words, some data could be neglected by monitoring system since that does not bring much new information for the users, thus it's not optimal to have them consume resources. Such change could affect how many subsystems could be monitored and server by monitoring system. 

Also, data should be stored in such data storage which is optimal for such workloads - huge amounts of data where more data coming in each second, but much less being read. Nevertheless, data must be optimized in a way that they would be easily presentable taking into account some filtering, ordering, aggregation, et cetera. It means that it's important to index data and maintain corresponding indexes per each update of data storage. As an example of such storage, ElasticSearch is used as industry standard for storing monitoring data. 

  https://i.gyazo.com/cb3673bbc7ee250d296ba0f415842dba.png
Picture X

Presentation: 
It's a stage where data is getting to the user of monitoring system. Collected and processed data is displayed to users in corresponding relevant form.
Considering the amount of data that distributed system outputs for monitoring purposes, the user of monitoring system should be able to keep up with such cognitive load. That's why the data must be optimized so the user would only explore what's relevant to him. Filtering, ordering, aggregation is used for such purpose.
Filtering data is the process of reducing data in a way that user of monitoring system could receive only data relevant for his level of perception (e.g., get information on what was happening at concrete server for last 15 minutes). Important thing to mention is that data stays in the data storage, but user interface only displays data relevant to user.
Moreover, there are other instruments of visualization which could reduce cognitive load. With the correct approach to visualization, use is able to explore the data for long period of time which is adapted so it's easier to percept it.

Already knowing monitoring stages, we can also present the three pillars of monitoring [5] - logs, metrics, and traces. They are deeply described in full work.

# Implementing monitoring

# Visualizing the results 

## Preventive safety via monitoring in distributed systems

# Conclusions

# Bibliography

1. Mansouri-Samani, Masoud & Sloman, Morris. (1993). Monitoring distributed systems. Network, IEEE. 7. 20 - 30. 10.1109/65.244791.

2. IEEE: Ieee standard glossary of software engineering terminology (1990), https://ieeexplore.ieee.org/document/159342

3. Manzo, M. & Frisiani, Arrigo & Vernazza, T.. (1982). A monitoring distributed system. Microprocessing and Microprogramming. 10. 19–24. 10.1016/0165-6074(82)90118-1. 

4. https://en.wikipedia.org/wiki/Observer_effect_(physics)
5. Distributed Systems Observability by Cindy Sridharan, https://www.oreilly.com/library/view/distributed-systems-observability/9781492033431/ch04.html#:~:text=Logs%2C%20metrics%2C%20and%20traces%20are,the%20three%20pillars%20of%20observability