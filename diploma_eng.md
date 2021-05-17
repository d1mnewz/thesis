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

## Monitoring distributed systems

# Implementing monitoring

# Visualizing the results 

## Preventive safety via monitoring in distributed systems

# Conclusions

# Bibliography

Mansouri-Samani, Masoud & Sloman, Morris. (1993). Monitoring distributed systems. Network, IEEE. 7. 20 - 30. 10.1109/65.244791.

IEEE: Ieee standard glossary of software engineering terminology (1990), https://ieeexplore.ieee.org/document/159342