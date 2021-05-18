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

Кожна з описаних вище компонентів необхідна, аби розподілена система стала оглядовою. Це було зроблено в розробленій нами програмі, яка покликана показати практичні приклади використання моніторингу в розподілених системах в контексті превентивної безпеки.
В розробленій програмі було використано такі технології як .NET Core, Docker, Docker-Compose, Prometheus, Kibana, ElasticSearch та Logstash. Кожен з цих компонентів відповідає за певну частину моніторингу.

.NET Core консольний додаток є ...
full code

Docker-compose to aggregate
https://github.com/d1mnewz/thesis/blob/f1c7e24b8b8d464574ff0d1d9000aa7bb10ba4f1/docker-elk-main/docker-compose.yml#L1
Docker, Docker-compose, each line in file.
yaml

Prometheus - це моніторингове рішення з відкритим кодом, що широко застосовується для збору, обробки та презентації даних пов'язаних з метриками. Першочергово, це рішення було розробленим в компанії SoundCloud, а згодом його зробили доступним для загального користування за ліцензією Apache License 2.0.
Після його створення в 2012 багато компаній та організацій почали ним користуватись, адже проєкт мав велику спільноту навколо нього, що означало підтримку. Так само, Prometheus було додано до Cloud Native Computing Foundation у 2016, що підтверджує його статус стандарту в індустрії.
Для того щоб розгорнути Prometheus у відповідному середовищі було використано Docker-Compose як найоптимальніший спосіб для швидкого розгортання, що не залежить від стану середовища.  
У наступних додатках можна побачити конфігураційні файли для Prometheus, що виконані на мові YAML. Ці конфігураційні файли впливають на те, як буде розгорнутим Prometheus та якою буде його поведінка у середовищі.

https://github.com/d1mnewz/thesis/blob/f1c7e24b8b8d464574ff0d1d9000aa7bb10ba4f1/docker-elk-main/docker-compose.yml#L11
Додаток х.
https://github.com/d1mnewz/thesis/blob/f1c7e24b8b8d464574ff0d1d9000aa7bb10ba4f1/docker-elk-main/prometheus/prometheus.yml#L1
Додаток х.
https://github.com/d1mnewz/thesis/blob/f1c7e24b8b8d464574ff0d1d9000aa7bb10ba4f1/docker-elk-main/prometheus/alert.yml#L1
Додаток х. alert.yml визначає, які сповіщення Prometheus мусить відслідковувати і повідомляти користувачам, якщо такі стануться.

Агрегуючи усі наявні метрики, наш програмний продукт відкриває їх через API endpoint "/metrics". Зробивши запит до цього API endpoint, можна побачити наступну відповідь:

```
# HELP process_open_handles Number of open handles
# TYPE process_open_handles gauge
process_open_handles 461
# HELP process_working_set_bytes Process working set
# TYPE process_working_set_bytes gauge
process_working_set_bytes 51826688
# HELP process_num_threads Total number of threads
# TYPE process_num_threads gauge
process_num_threads 25
# HELP prom_warning This fields indicates the warning count.
# TYPE prom_warning counter
prom_warning 3
# HELP process_cpu_seconds_total Total user and system CPU time spent in seconds.
# TYPE process_cpu_seconds_total counter
process_cpu_seconds_total 2.453125
# HELP process_private_memory_bytes Process private memory size
# TYPE process_private_memory_bytes gauge
process_private_memory_bytes 34926592
# HELP process_start_time_seconds Start time of the process since unix epoch in seconds.
# TYPE process_start_time_seconds gauge
process_start_time_seconds 1621152828.4403536
# HELP dotnet_collection_count_total GC collection count
# TYPE dotnet_collection_count_total counter
dotnet_collection_count_total{generation="1"} 0
dotnet_collection_count_total{generation="2"} 0
dotnet_collection_count_total{generation="0"} 0
# HELP prom_exception This fields indicates the exception count.
# TYPE prom_exception counter
prom_exception 2
# HELP prom_ok This fields indicates the transactions that were processed correctly.
# TYPE prom_ok counter
prom_ok 131
# HELP process_virtual_memory_bytes Virtual memory size in bytes.
# TYPE process_virtual_memory_bytes gauge
process_virtual_memory_bytes 2203929231360
# HELP dotnet_total_memory_bytes Total known allocated memory
# TYPE dotnet_total_memory_bytes gauge
dotnet_total_memory_bytes 4218768
```
Додаток X. Відповідь API endpoint, що дає розроблений програмний продукт.

###### Візуалізовуючи результати моніторингу


# Visualizing the results 

Prometheus відображдає продуктивність системи у вигляді графу, що дозволяє користувачам у доступному їм форматі легко зрозуміти стан системи. Інформація подана в форматі графіків дозволяє легко порівнювати продуктивність системи з різних перспектив, а також оцінювати її відносно часу.
Задля інтерактивної візуалізації використовується Prometheus Expression Browser, що надає ефективний спосіб відображати великі об'єми метрик зібраних за певний час. Ця частина системи є доступною за шляхом "http://localhost:9000/new/graph". За допомогою Prometheus Expression Browser можна дослідити дані, вибираючи конкретні метрики, що користувач хоче побачити на графіку.

https://cdn.buttercms.com/Hm6hrn2nRT2VznSRd13m
Рисунок х. Приклад користування Prometheus
https://cdn.buttercms.com/z3am8gfBTQKIjdvAOQKQ
Рисунок х. Приклад користування Prometheus

Рисунок x. тг
Дослідивши даний графік, вже можна зробити висновки, що програма не є стабільною, адже очевидними є падіння в графіку, що репрезентують вимкнення програми.
Так само, варто наголосити на тому, що графік показує лінійний ріст вимірюваної метрики - "prom_ok".


## Preventive safety via monitoring in distributed systems

Це звучить логічно, що покращувати стабільність системи має сенс для користувачів системи та для бізнесу. Тим не менше, це парадоксально - в деяких випадках стає лише гірше!

Максимальна стабільність має свою ціну у вигляді довшого часу на розробку нових можливостей системи, довший час на доставку системи до користувачів. Очевидно, ця ціна напряму корелює з бюджетними обмеженнями, відповідно, менше можливостей системи буде імплементовано.

Порівнюючи такі SLA як 99%, 99.99% та 99.999% для певної системи, можна стверджувати, що користувач зазвичай не помітить різниці, адже його користувацький досвід стосовно стабільності буде більше сфокусований на менш стабільних факторах таких як сучасний смартфон, що забезпечує 99% SLA.

Table 3. Impact of downtime duration on availability metric.
Downtime per month Downtime per year Availability %
72 hours 36.5 days 90% ("one nine")
7.20 hours 3.65 days 99% ("two nines")
43.8 minutes 8.76 hours 99.9% ("three nines")
4.38 minutes 52.56 minutes 99.99% ("four nines")
25.9 seconds 5.26 minutes 99.999% ("five nines")


Розуміючи ці фактори, при моделюванні систем ми мусимо глибоко розуміти доменну область і бізнес-модель проєкту, адже для соціальної мережі обміну картинками котиків абсолютно не обов'язково інвестувати в 99.999% SLA, проте для системи обробки запитів до невідкладної допомоги 9 годин відсутності можливості приймати запити є критичним та напряму впливає на життя та здоров'я користувачів системи. Тому, при виборі SLA, ми мусимо балансувати між ризиком недоступності системи (і шкодою, яка може бути нанесена цією недоступністю) та інноваційним розвитком системи, що створює нові можливості для своїх користувачів.
Для того, аби змоделювати систему прийняття такого рішення, розглянемо наступний приклад, де кожен запит приносить однакову цінність:

Пропозиція стосовно покращення SLA: 99.9% -> 99.99%
Пропоноване покращення в стабільності: 0.09%
Прибуток, який приносить система: 1 млн. дол.
Фінансова цінність покращення: 1 млн. дол. * 0.0009 = 900$

FTODO: ^ в таблицю.

В цьому випадку, якщо вартість створення запропонованого покращення буде менша за 900$, то це має сенс імплементувати. Якщо вартість покращення буде вищою за 900$, то вартість покращення буде вищою за інвестицію, відповідно прибуток не збільшиться. Зазвичай, вартість такого покращення буде надзвичайно високою, адже покращення саме на верхній межі є найдорожчими з точки зору ресурсів необхідних на імплементацію.

Так само, розглядаючи ціну моніторингу, варто згадати, що моніторинг вимагає впровадження, розробку, конфігурацію та підтримку додаткових систем. Окрім цього, це створює додаткове когнітивне навантаження на розробників, що займаються розробкою та підтримкою розподіленої системи. Відповідно, необхідно більше ресурсів витрачати на те, аби документувати підходи та навчити розробників користуватись такою моніторинговою системою.

Тим не менше, інвестицію в навчання працівників можна оптимізувати таким чином, аби не витрачати на це час щоразу, коли з'являється новий працівник. Гарним рішенням подібної проблеми може бути уніфікація моніторингової інфраструктури та моніторингових вимог від частин розподіленої системи. У більшості випадків, моніторинг застосовується до різних платформ таких як .NET, Java, Go, тощо.
Створивши документ, який описує найкращі практики до застосування моніторингових інструментів, з'являється можливість легкого навчання працівників, а також перевикористання цих найкращих практик у частинах розподіленої системи. Це стосується не лише концептуальних підходів, а й вже готових рішень для перевикористання: конфігурацій метрик, списку залежностей, тощо.
Так само, окрім документації таких підходів, ще варто приділити увагу створенню базового прикладу сервісу, що береться за відправну точку для кожного нового сервісу в розподіленій системі. Такий базовий приклад включає в себе всі найкращі практики, які дозволяють вже бути готовими до використання в production середовищах з першого дня існування. Варто наголосити, що основна причина для такої документації - це оптимізувати використання часу в командах продуктових розробників, аби вони могли більше фокусуватися на продукті, а не на інструментації, моніторингу, чи налаштуванні інфраструктури.

Так само, осмислюючи моніторинг, варто врахувати, що деякий відсоток обчислювальних ресурсів системи буде витрачатись на моніторинг. Відповідно, це буде відображено у загальній продуктивності системи. Деякі розподілені системи можуть сягати тисяч серверів, тому очевидно, що ресурси витрачені на моніторинг та оглядовість таких систем можуть мати великий вплив на продуктивність розподіленої системи. Аби обслуговувати такі розподілені системи, моніторингова система має споживати якомога менше ресурсів, наскільки це можливо, але не менше, аби не спричинити втрати даних або власну недоступність.

Ще один фактор, що є частиною ціни і вартий того, аби його розглянути в даній роботі, це великий та постійно ростучий розмір артефакту, який створений моніторинговою системою. Очевидно, що обсяг даних, що збирається з розподіленої системи, є таким, що важко опрацювати, тому критично важливим є оптимізація таких даних для сприйняття користувачем. Так само, цей масив даних необхідно зберігати. В деяких системах є допустими видаляти такі дані після певного періоду зберігання.

Кілька тез у якості висновку:
- Надійність та стабільність систем має багато чого спільного з вимірюванням ризиків і прийманням обдуманих рішень. Аби вимірювати ризики, необхідно розуміти систему, продукт, а також вплив на користувачів. Вимірювання ризиків зазвичай є дорогим інструментом саме по собі.
- Рівень доступності системи має бути підібраний відповідно до того, що бізнес може собі дозволити - з точки зору ціни та впливу помилок на користувачів. Більшість систем не потребують SLA 99.999% та більше.


##### превентивна безпека за допомогою моніторингу

The work describes possible negative outcomes for software product and defines what are the problems to be prevented by monitoring:
- user dissatisfaction with the product
- harm to users or their property (e.g., data)
- loss of trust
- direct or indirect income loss
- negative pressure on brand name
- undesired mentions in media
- suboptimal usage of computing resources, therefore suboptimal financial strategy

Most of these points are highly related to business outcomes since the business is the one affected by anything happening with distributed system. Also, some of these points are hardly measurable.
So, everyone expects that system is going to be stable with no losses, no problems, and will gain profit to the company, and added value to its users. 

The work already discussed some approaches to monitoring in previous chapters. Nevertheless, they all cover only identification and investigation of problems once incident has happened or is happening at the moment.
Instead, this part of the research focuses on incidents before they actually happen. It focuses on preventing them from happening. Correspondingly, some of the approaches to preventive safety are similar to approaches for solving incidents in production systems. 
Monitoring allows to see the negative trends in systems performance which, in its turn, allows them to take appropriate actions for reacting upon these trends. 
In the context of preventive safety there is no direct need to react to these trends in the moment of identifying them. Instead, it makes more sense to analyze them and then prioritize only after figuring out their potential damage.
Also, special attention should be payed to warnings. The codebase should be enriched with more logging, additional metrics which follow the same goal - to define granularity of system processes. In other words, it's important to understand situations where it's not an error yet, but at the same time it's not a happy-path scenario, subject to worry about. 
Based on monitoring data presented in a form of dashboards and trends, system developers and product owners could take thought out decisions. There is always a spectre of available decisions to take. Samples are illustrated in the following table. 

FTODO: Таблиця feedback loop cycle:
Data analysis -> Decision & Action
CPU slowly goes up during last month -> Developers decide to scale up so the service has more CPU available. It reduces the risk of incident for short period of time
Service saturation goes down -> Developers take decision to scale down by having weaker server for this service. It optimized resource usage and financial strategy
Database is heavily loaded, 2% requests fail -> Developers change the architecture by adding caching in front of database

Also, the work discovers preventive safety for software products from managers point of view. 
Key points there:
- Managers are interested in system being stable so it can serve users
- There is SLA which systems have to honour as it sets expectations. It's important to set realistic expecations in this case
- Different domains require different SLAs
- Managers want to make a data-driven decision on stability
- Non-functional requirements are as important as functional for high-load systems
- The main goal for manager is to balance the feature delivery and investment into system reliability

Also, the work discusses the importance of creating monitoring culture and monitoring mindset which largely affects the software product being built. 

Additionally, postmortem culture is discussed in the work as an instrument of preventive safety. While referring to postmortems, we also discuss Corrective and Predictive Actions (CAPA) as a concept from which postmortems are inherited. 
So, postmortem document is a document regarding the incident happened which answers following questions:
- What happened?
- How system and developers reacted?
- What could we do differently next time?
- What could be done to prevent this from happening?

The work focuses on last two questions since they are critical for understanding preventing the danger either by avoiding it, or fighting it more effectively. The work also stresses on sharing such postmortems so everyone could get a clear understanding of what has happened. That way, knowledge flows across the organization and prevents the happening of new incidents. In the end, postmortems are the potential to strenghten the distributed system and system developers.

# Conclusions
Monitoring is essential part of modern distributed systems for visualizing, testing, debugging and development. With no monitoring and observability modern distributed system have miniumum chances to succeed and to be stable. Taking into account this and other researches, it was identified that monitoring and observability of distributed systems are not that much of a technical issue anymore. Instead, it becomes a cross-concern topic which goes through whole software lifecycle, and, therefore, is critical for product success. 

The work described distributed systems as field of study along with monitoring for better understanding of the domain. Also, the work exposed the problem of monitoring by describing the three pillars of monitoring - logs, traces and metrics. These pillars were presented both conceptually and with examples provided by softaware product built for illustrativeness. Mostly the work is dedicated to preventive safety as a goal, and monitoring as an instrument for achieving that goal. Thinking through monitoring, we also covered managerial point of view on that topic. Finally, we have covered postmortems as an approach to preventing incidents as well.
Future works are presented in the following chapter.

## Future work
We suggest discovering the usage of Artificial Intelligence for anomaly detection since that's the area which has the most potential, and still not studied well. Such application might automate a lot of processes related to defining stable level of the system health. For instance, AI could be used for automatic defining of predicates to be used in alerts for distributed systems. Similarly, it could be used for identifying anomalies which are not involved in the alerts system, but, nevertheless, are relevant for discoverting the state of the system and might affect the stability of the system. Once the nature of these anomalies is well understood, it's possible to prevent the incidents. Another important usage of AI in monitoring systems could be automatic reducing of data presented to the user for investigation so it reduces the cognitive load. Obviously, there is some data to be neglected.
Also, the most interesting area of applying AI and machine learning is predicting the root cause of incidents based on available data.

We did not study deeply how monitoring affects the performance of distributed system so this could also be another topic for research. This is important since the technologies for monitoring change so fast so there is risk of reducing the performance of the system upon which monitoring is applied. 

In this work we focused on modern approaches to monitoring with regards to preventive safety in distributed systems. Obviously, the modern world is evolving extremely rapid, especially in IT, thus it might be a great idea to create a research on changes which happen in preventive safety via monitoring in distributed systems. 

# Bibliography

1. Mansouri-Samani, Masoud & Sloman, Morris. (1993). Monitoring distributed systems. Network, IEEE. 7. 20 - 30. 10.1109/65.244791.

2. IEEE: Ieee standard glossary of software engineering terminology (1990), https://ieeexplore.ieee.org/document/159342

3. Manzo, M. & Frisiani, Arrigo & Vernazza, T.. (1982). A monitoring distributed system. Microprocessing and Microprogramming. 10. 19–24. 10.1016/0165-6074(82)90118-1. 

4. https://en.wikipedia.org/wiki/Observer_effect_(physics)
5. Distributed Systems Observability by Cindy Sridharan, https://www.oreilly.com/library/view/distributed-systems-observability/9781492033431/ch04.html#:~:text=Logs%2C%20metrics%2C%20and%20traces%20are,the%20three%20pillars%20of%20observability