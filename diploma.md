# титульний аркуш

Monitoring and observability of distributed systems in the context of preventive safety

# завдання на бакалаврську роботу
- реквестнути в керівника

# анотація (укр та англ)

Моніторинг та оглядовість включають в себе збір, обробку, аналіз та візуалізацію різноманітної динамічної інформації стосовно певного процесу. 
У сучасних розподілених системах вже доступна велика кількість напрацювань, аби полегшити ці три нетривіальні завдання, проте моніторинг досі одна з найскладніших та найважливіших частин моделювання програмних продуктів у production середовищах.
Робота розкриває причини такого широкого поширення розподілених систем серед розробників та тих, хто моделює системи, наголошуючи на доступності таких практик як DevOps, SRE, тощо.

Робота також визначає характеристики розподілених систем, що роблять їх моніторинг таким нетривіальним. Ці характеристики використані задля розуміння методології моніторингу та порівняння доступних досліджень на цю тематику.

Сама робота має на меті дослідити важливість моніторингу та оглядовісті в сучасних розподілених системах, зважаючи на доменну область системи в якій застосовані, а також дослідити збір, обробку, аналіз, та презентацію операційних даних в розподілених системах задля запобігання критичним ситуаціям. Тим не менше, критичні ситуації стаються та з них треба відновлюватись, тому робота в тому числі описує й способи відновлення після інцидентів та як це впливає на запобігання наступним інцидентам. Так само, робота розкриває ціну моніторингу на кожному етапі життєвого циклу програмного продукту, з наголосом на ціну моніторингу саме в production середовищах. 

Кожна частина цього дослідження буде розглянута як концептуально, так і на практиці, підтверджуючи теорію прикладами створених систем та їх оглядовості: метрик, сповіщень, логування, графіків, повідомлень, відлагодження, трасування. Ці приклади виконані з урахуванням доступних сучасних інструментів таких як Azure Application Insights, AWS Cloudwatch, Kibana, ElasticSearch, Logstash, Zipkin, OpenTelemetry, та інші. 


Інформація подана в цій роботі базується на досвіді дослідника стосовно розробки та проектування розподілених систем, а також на досвіді підтримки таких систем в production середовищах з фокусом на стабільність та безпеку.

Ключові слова: MONITORING, OBSERVABILITY, DISTRIBUTED SYSTEMS, SOFTWARE SAFETY, SOFTWARE ENGINEERING, PREVENTIVE SAFETY IN SOFTWARE

Анотація англійською.

# зміст

# перелік умовних позначень, символів, одиниць, скорочень і термінів
IEEE defines monitoring as the supervising, recording, analyzing or verifying
the operation of a system or component [10].
The term Observability originates in control system theory and measures the
degree to which a system’s internal state can be determined from its output
[7]. In cloud environments, observability indicates to what degree infrastructure
and applications and their interactions can be monitored. Outputs used are for
example logs, metrics and traces [18].
Моніторинг

Обсервабіліті

Розподілена система

Production система

serverless функції

Dashboard 

SLA (37 сре бук)
AOP

Docker
Prometheus

Happy-path сценарій - 
JSON - 
# вступ

робота скіпає моніторинг для бізнес та секьюріті.

Ця робота організована наступним чином: розділ 1 описує область дослідження - розподілені системи, а також знайомить читачів з поняттями моніторингу. У розділі 3 описано підходи до створення програмних продуктів, чому сучасні системи будують як розподілені, та які основні проблеми мають такі системи. Моніторинг для розподілених систем презентований у розділі 4. Превентивна інструменти моніторингу висвітлені у розділі 5. Розділ 6 описує економічну частину роботи. Розділ 7 присвячений висновкам.

## опис домену
Областю дослідження в цій роботі однозначно є розподілені системи. Перед тим, як ми заглибимось в підходи, термінологію, інструменти, важливим є окреслити сучасну суть розподілених систем, та як вони еволюціонували з часом, адже поняття вже є сталим в індустрії ще з 1980-х років. 
Тим не менше, можна стверджувати що й визначення розподілених систем з 1980-х цілком може підійти й для сьогодення. 
Розподілена система складається з певної кількості процесів, що працюють на різних серверах та виконують спільну роботу. Очевидно, що ця спільна робота є розподіленою між серверами задля оптимальних сценаріїв виконання. Ці процеси мусять координуватись між собою, комунікуючи за допомогою протоколів комунікації (напр. HTTP, AMQP, gRPC, тощо) .[Coulouris, 2011; Garg, 2002; Attiya and Welch, 2004].
Системи є розподіленими за своїм дизайном ще здавна, адже наприклад багатопотічна програма є також розподіленою системою, хоч і робота в ній розподілена поміж потоками та ядрами. 

В рамках останнього десятиліття (2010-2020) розподілені системи стали трендом в розробці високонаватажених систем. Варто зазначити, що сучасні розподілені системи є найбільш розподіленими, яких лише бачив світ, адже вони захоплюють різні сервери, хмарні провайдери, розташовані в різних країнах, контрольовані різними компаніями, виконують різноманітні програми, що базуються на різних платформах, тощо. 
Навіть коли кожна окрема підсистема є тривіальною та простою, інтеграція цих компонентів разом може створювати неймовірну складність. Це саме по собі створює низку проблем, яких не було в традиційних системах, що працювали в рамках одного сервера. 
The number of pieces in play increases the statistical likelihood of failure.


Ці проблеми включають в себе, але не обмежуються:
- Різноманітні компоненти можуть неочікувано вимикатися незалежно один від одного, непередбачувано реагувати на проблеми в інших частинах системи
- Комунікація між частинами системи може бути провальною, в тому числі й через тимчасові проблеми мережі. В таких середовищах, якість мережі та її стабільність відіграють важливу роль.
- Якщо мережеві проблеми стаються, то починаються повторення запитів / повідомлень, що може спричиняти неочікуваний та некоректний стан системи у кінці-кінців
- Гетерогенність систем, адже вони можуть складатися з кількох рівнів  - код, що виконує бізнес-сценарії, інфраструктурні технології такі як Docker контейнери, віртуальні машини, або навіть serverless функції. Більше того, команди для таких систем також можуть бути гетерогенними. В той час, коли таке середовище може культивувати іновації, дозволяючи різним командам вибирати різні технології для своїх частин системи, з іншого боку це ж і створює проблематику для консистентного підходу до моніторингу таких систем.

Відповідно, відслідковування трендів, та огляд розподілених обчислень й систем є важливою проблемою ще з ранніх часів розподілених систем. [Cooper and Marzullo, 1991].  

Already in the late 1980s, Joyce et al. [Joyce et al., 1987] identified five issues
in monitoring distributed systems, in an early attempt to characterize the key
constraints that distinguish monitoring in sequential settings from monitoring
in distributed systems:
(C1) The fact that distributed systems have many foci of control;
(C2) The presence of communication delays among nodes, which makes it difficult to determine a system’s state at any given time;
(C3) The inherent non-determinism in distributed and asynchronous systems;
(C4) The fact that monitoring a distributed system alters its behavior ;
(C5) The complexity of the interactions between the system and the system
developer.



(референс на https://www.researchgate.net/publication/3282450_Monitoring_distributed_systems )


Чому так сталося? Відповідь на це питання досить проста, хоч і складається з кількох факторів, що зійшлись разом:
- Доступність 
	- Контейнери, Docker, Kubernetes, дешевий Cloud Computing 
- 
The growing popularity of cloud computing, big data clusters, and instance orchestration layers has forced operations professionals to rethink how to design monitoring at scale and tackle unique problems with better instrumentation. 


In order to model and mirror the systems it watches, monitoring infrastructure has always been somewhat distributed. However, many modern development practices—including designs around microservices, containers, and interchangeable, ephemeral compute instances—have changed the monitoring landscape dramatically.

Тим не менше, з тим як розподілені системи стають все більш поширеними серед розробників та тих, хто проектує системи, постала логічна потреба в моніторингу таких систем, адже кожен розробник та менеджер хочуть мати якомога більше розуміння та контролю над такою системою задля покращення її якостей. Тому, за такими системами необхідно слідкувати у всіх стадіях їхнього життєвого циклу - від розробки до щоденної підтримки production системи. 

The monitoring system in this case takes on a central role in controlling the environment and deciding when to take action.

the tendency for modern architectures to divide up work and functionality between many smaller, discrete components. These designs can have a direct impact on the monitoring landscape because they make clarity and comprehensibility especially valuable but increasingly elusive.

More robust tooling and instrumentation is required to ensure good working order. However, because the responsibility for completing any given task is fragmented and split between different workers (potentially on many different physical hosts), understanding where responsibility lies for performance issues or errors can be difficult. Requests and units of work that touch dozens of components, many of which are selected from pools of possible candidates, can make request path visualization or root cause analysis impractical using traditional mechanisms.


Моніторинг - що таке моніторинг
Обсервабіліті
- потреба це debugging tomorrow та виявлення закономірностей 
основна різниця між монінторингом і обсервабіліті
системи зараз розподілені 
(картинка з двома компами в системі які 55% і один падає і весь лоад на інший - нестабільно)
і щоб зробити таке твердження - ми маємо знати який лоад зараз. 

моніторинг of things you do not know yet

обсервабіліті - інвестиція, що робиться в майбутнє. 

превентивна безпека
кейси де превентивна безпека могла б врятувати


# основна частина
## Дотичні дослідження

Фундаментальні для області дослідження висвітленої в цій роботі стали роботи ... , що 

Є також сучасні дослідження, які розкривають аспекти моніторингу, що є прикладними до сьогодення, описуючи інструменти та підходи, що вже використовуються серед розробників систем. Гарним прикладом такого дослідження є "On Observability and Monitoring of Distributed Systems – An Industry Interview Study" від Sina Niedermaier, Falko Koetter, Andreas Freymann, Stefan Wagner. Такі дослідження менше фокусуються на фундаментальних концепціях, натякаючи, що читачі вже знайомі з усією термінологією, а основою дослідження є прикладні історії та висновки з індустрії.  

To provide context to the survey described in this work, the related work investigates (1) current approaches to bridging the gap between distributed system
complexity and monitoring capability as well as (2) preceding surveys regarding
monitoring and observability (see Figure 1).
IEEE defines monitoring as the supervising, recording, analyzing or verifying
the operation of a system or component [10].
The term Observability originates in control system theory and measures the
degree to which a system’s internal state can be determined from its output
[7]. In cloud environments, observability indicates to what degree infrastructure
and applications and their interactions can be monitored. Outputs used are for
example logs, metrics and traces [18].
Yang et al. [24] investigate the capturing of service execution paths in distributed systems. While capturing the execution path is challenging, as each
request may cross many components of several servers, they introduce a generic
end-to-end methodology to capture the entire request



Превентивний аспект моніторингу розподілених систем не був досі широко висвітленим серед доступних досліджень, тому й ця робота покликана заповнити цю прогалину, аналізуючи існуючі дотичні роботи, та осмислюючи як моніторинг може запобігати інцидентам та різноманітним неочікуваним проблемам.

### Проблеми з програмними продуктами на стадії підтримки 

Можна виділити два типи проблем із програмними продуктами на стадії підтримки - операційні проблеми та програмні проблеми. 
До операційних проблем ми можемо віднести мережеві проблеми (у всій країні вимкнули інтернет), перегрів CPU через спеку, проблеми з hardware - диск перестав працювати, бо на нього впав камінь в серверній - це все проблеми, які варто вирішити за допомогою зовнішнього впливу. Цим зовнішнім впливом можуть стати такі прості речі як переставити сервер в прохолодний підвал, або замінити привід диску.

Є ще один тип проблем - це програмні проблеми. Простіше кажучи, це проблеми спричинені тими чи іншими розробниками програм. Це проблеми всередині логіки програми. 
Аби вирішити такі проблеми, розробники мусять до інциденту зібрати якомога більше інформації стосовно того, що відбувалось в системі, і це дозволить їм сформувати ланцюжок причинно-наслідкових зв'язків, які допоможуть їм знайти джерело проблеми та виправити його. В таких ситуаціях моніторингові дані - це критична необхідність, а не додатковий аспект системи. Даними можуть бути різноманітні метрики, логи, трейси, тощо. 



Парадоксально, проте чим більшу інвестицію проєкт робить в оглядовість, тим менше часу доведеться за ним наглядати, тим більше часу можна буде виділити на розробку проекту. 

#### програмні продукти як предмет для відслідковування тривожних тенденцій

Одним з важливих завдань в моделюванні оглядовості для розподілених систем є створення середовища, де розробники могли б приймати рішення базуючись на інформації (метрики, сповіщення, тощо) про стан системи. Інакше, якщо при моделюванні цей аспект врахований не буде, то розробникам в певний момент будуть очевидними проблеми із системою (скарги користувачів, витоки даних, некоректно відпрацьовані сценарії, тощо), проте це вже буде занадто пізно - шкода буде зроблена для користувачів та зацікавлених осіб, що може бути критичним. Саме в цьому й полягає основне завдання оглядовості розподілених систем - попередити критичну ситуацію, аби розробники могли прийняти рішення і не дати цьому статися. Саме це й є превентивною безпекою в даному випадку.

#### програмні продукти та їхні дефекти

кейси
...
кейси

##### Моніторинг та його компоненти
Багато з описаних вище проблем можна було б вирішити за допомогою сучасних підходів до моніторингу.

Що таке моніторинг та оглядовість. Що таке інцидент.
Після інциденту роблять post-mortem.  Пост-мортем детальінше описаний в розділі .... 
Основне призначення обсервабіліті - це під час інциденту дати більше інформації для прийняття рішення.

Цей розділ описує 3 основні етапи з яких і складається моніторинг розподілених систем - збір даних, обробка та презентація.

Створення та збір даних:
Релевантні дані з системи мусять бути сформовані в зрозумілий формат для системи збору цих даних. Наприклад, Prometheus збирає дані з сервісу і репрезентує їх через endpoint у наступному форматі:

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

Відповідно, ці дані вже відповідають очікуваному формату, і можуть бути зібрані. 
Так само, варто згадати, що збір даних має забирати якомога менше ресурсів у production системи та не заважати її нормальному функціонуванню. Детальніше про це описано у розділі ...

Обробка: 
- В деяких випадках даних може бути більше, аніж достатньо, але великого інформаційного навантаження вони не несуть - напр. з певних сервісів немає сенсу збирати щосекунди інформацію про навантаженість їхнього CPU. В таких випадках, може бути застосовний такий підхід як адаптивний семплінг. Це зменшить кількість даних, які зберігаються на рівні сховища даних, проте це не буде критичним для розуміння стану системи. Простіше кажучи, деякими даними моніторинг система готова жертвувати, адже вони не несуть нової інформації для користувачів, а тому не такими, що достойні займати ресурси. Такі зміни впливають на кількість підсистем, які може моніторити та обслуговувати моніторинг система.

- дані зберігаються в сховищі даних, яке є оптимізованим для такого типу навантаження - великого об'єму даних, де багато даних додається щосекунди, проте набагато менше читається. Тим не менше, дані мають бути оптимізовані таким чином, аби їх можна було легко презентувати з урахуванням певних фільтрів. Це означає, що необхідно індексувати дані та підтримувати відповідні індекси при кожному оновленні бази даних. Прикладом сховища даних може бути ElasticSearch в такому випадку.


Презентація:
Це етап, де дані потрапляють до користувача моніторингової системи. Зібрані та проаналізовані дані представляються користувачам в формі, яка є доречною для кожного випадку.
Зважаючи на те, що розподілена система може створювати величезні обсяги інформації для аналізу та презентації, користувач моніторинг системи не в стані справитися з таким когнітивним навантаженням. Тому існує необхідність оптимізовувати ці дані до такого вигляду, що буде релевантним користувачу. Саме для цього існує фільтрування даних.
Фільтрування даних - це процес мінімізації даних таким чином, що користувач моніторингової системи зможе отримати лише необхідні дані для його рівня сприйняття (напр. отримати інформацію про все, що відбувалось на конкретному сервері за останні 15 хвилин). Варто зазначити, що дані досі лишаються в сховищі даних, проте користувацький інтерфейс показує лише те, що користувач робить запит. 

Так само, дуже важливо мати інструменти візуалізації, що допоможуть зменшити когнітивне навантаження на користувача, не зменшуючи кількість інформації, яку він може оглянути. Більше того, при коректній візуалізації, користувач має можливість ознайомитись з даними за довгі періоди часу, які адаптовані для легшого сприйняття в стислі рамки часу. 
Приклади імплементації візуалізації моніторингових даних можна побачити в розділі ...





Ознайомившись з етапами моніторингу, обов'язковим є наголосити на важливості даних. В розподілених системах виділяють три основні типи даних, що роблять можливим моніторинг такої системи:
- Логи
- Метрики
- Трейси

Також, цю трійку компонентів називають the three pillars (референс). Детальніше про кожен з типів даних є описано в наступних підрозділах.

###### Логи

Лог - це незмінний запис в моніторинговій системі, що описує факт, який стався в системі. Важливо зазначити, що цей запис має часову позначку, що презентує момент, коли цей факт стався. 

Логи можна поділити на два типи: текстові та структуровані. 
Текстові є найбільш поширеними. Структуровані є менш поширеними, але набирають все більше популярності, адже дозволяють структурований пошук та фільтрування по полях лог запису, який презентований в форматі JSON. Обидва є реалізовані в даній роботі.

Debugging rare or infrequent pathologies of systems often entails debugging at a very fine level of granularity. Event logs, in particular, shine when it comes to providing valuable insight along with ample context into the long tail that averages and percentiles don’t surface. As such, event logs are especially helpful for uncovering emergent and unpredictable behaviors exhibited by components of a distributed system.

Failures in complex distributed systems rarely arise because of one specific event happening in one specific component of the system. Often, various possible triggers across a highly interconnected graph of components are involved. By simply looking at discrete events that occurred in any given system at some point in time, it becomes impossible to determine all such triggers. To nail down the different triggers, one needs to be able to do the following:

Відповідно, у системах, де логуються вхідні та вихідні параметри, вони можуть бути використані для повторного виконання таких запитів та відлагодження реації системи на вхідні параметри в ізольованому середовищі

###### Метрики
Metrics are a numeric representation of data measured over intervals of time. Metrics can harness the power of mathematical modeling and prediction to derive knowledge of the behavior of a system over intervals of time in the present and future.

Since numbers are optimized for storage, processing, compression, and retrieval, metrics enable longer retention of data as well as easier querying. This makes metrics perfectly suited to building dashboards that reflect historical trends. Metrics also allow for gradual reduction of data resolution. After a certain period of time, data can be aggregated into daily or weekly frequency.

Advantages of Metrics over Event Logs
By and large, the biggest advantage of metrics-based monitoring over logs is that unlike log generation and storage, metrics transfer and storage has a constant overhead. Unlike logs, the cost of metrics doesn’t increase in lockstep with user traffic or any other system activity that could result in a sharp uptick in data.
###### Трейси

One of the most challenging aspects of highly distributed workloads is understanding the interplay between different components and isolating responsibility when attempting root cause analysis. Since a single request might touch dozens of small programs to generate a response, it can be difficult to interpret where bottlenecks or performance changes originate. To provide better information about how each component contributes to latency and processing overhead, a technique called distributed tracing has emerged.

Distributed tracing is an approach to instrumenting systems that works by adding code to each component to illuminate the request processing as it traverses your services. Each request is given a unique identifier at the edge of your infrastructure that is passed along as the task traverses your infrastructure. Each service then uses this ID to report errors and the timestamps for when it first saw the request and when it handed it off to the next stage. By aggregating the reports from components using the request ID, a detailed path with accurate timing data can be traced through your infrastructure.

This method can be used to understand how much time is spent on each part of a process and clearly identify any serious increases in latency. This extra instrumentation is a way to adapt metrics collection to large numbers of processing components. When mapped visually with time on the x axis, the resulting display shows the relationship between different stages, how long each process ran, and the dependency relationship between events that must run in parallel. This can be incredibly useful in understanding how to improve your systems and how time is being spent.

https://www.oreilly.com/library/view/distributed-systems-observability/9781492033431/assets/dsob_0404.png
Figure 4-4. A trace represented as spans: span A is the root span, span B is a child of span A


###### Реалізовуючи компоненти моніторингу
Кожна з цих частин необхідна, аби розподілена система стала оглядовою. Відповідно, це необхідно імплементувати. 

reference Distributed Systems Observability by Cindy Sridharan https://www.oreilly.com/library/view/distributed-systems-observability/9781492033431/ch04.html#:~:text=Logs%2C%20metrics%2C%20and%20traces%20are,the%20three%20pillars%20of%20observability.


###### Візуалізовуючи результати моніторингу
- візуалізація
	- з роками, моніторинг системи еволюціонували і створили власні мови для маніпуляції даними та презентації їх в вигляді графів, чартів, сповіщень, дашбордів.   
		- алерти
		- дашборди
		- лог агрегація і пошук  
	- імпл - Ажуре, ELK, AWS, тощо
		- цікаво, але для розподіленої системи оптимальним є використати систему для моніторингу, яка, в свою чергу, теж є розподіленою. розгляньмо приклад ELK stack  
		- як готувати обсервабліті в Ажуре 

Які є способи візуалізації даних, що можуть оптимаьно презентувати моніторингові дані:
- Гістограми
- Граф
- Текст
- Часові діаграми
- Pie-charts 
- 


##### ціна моніторингу як інструменту забезпечення стабільності

Це звучить логічно, що покращувати стабільність системи має сенс для користувачів системи та для бізнесу. Тим не менше, це парадоксально - в деяких випадках стає лише гірше! 

Максимальна стабільність має свою ціну у вигляді довшого часу на розробку нових можливостей системи, довший час на доставку системи до користувачів. Очевидно, ця ціна напряму корелює з бюджетними обмеженнями, відповідно, менше можливостей системи буде імплементовано. 

Порівнюючи такі SLA як 99%, 99.99% та 99.999% для певної системи, можна стверджувати, що користувач зазвичай не помітить різниці, адже його користувацький досвід стосовно стабільності буде більше сфокусований на менш стабільних факторах таких як сучасний смартфон з 99% SLA. 

Розуміючи ці фактори, при моделюванні систем ми мусимо глибоко розуміти доменну область і бізнес-модель проєкту, адже для соціальної мережі обміну картинками котиків абсолютно не обов'язково інвестувати в 99.999% SLA, проте для системи обробки запитів до невідкладної допомоги 9 годин відсутності можливості приймати запити є критичним та напряму впливає на життя та здоров'я користувачів системи. Тому, при виборі SLA, ми мусимо балансувати між ризиком недоступності системи (і шкодою, яка може бути нанесена цією недоступністю) та інноваційним розвитком системи, що створює нові можливості для своїх користувачів.
Для того, аби змоделювати систему прийняття такого рішення, розглянемо наступний приклад, де кожен запит приносить однакову цінність:

Пропозиція стосовно покращення SLA: 99.9% -> 99.99%
Пропоноване покращення в стабільності: 0.09%
Прибуток, який приносить система: 1 млн. дол.
Фінансова цінність покращення: 1 млн. дол. * 0.0009 = 900$

В цьому випадку, якщо вартість створення запропонованого покращення буде менша за 900$, то це має сенс імплементувати. Якщо вартість покращення буде вищою за 900$, то вартість покращення буде вищою за інвестицію, відповідно прибуток не збільшиться. Зазвичай, вартість такого покращення буде надзвичайно високою, адже покращення саме на верхній межі є найдорожчими з точки зору ресурсів необхідних на імплементацію.  

- підтримка додаткових систем, додатковий когнітів лоад на інженерів, що займаються розробкою та підтримкою
- створює багато даних
	- data retention
	-  
- величезний моніторинг артефакт
	- the overwhelming flood of data coming from the distributed system, which is constantly in change. The identified challenge is to create meaningful conclusions from customer alerts and how to prioritize them. 
- важко навчати працівників
- необхідно уніфікувати моніторинг, який працює з різними платформами (.NET, Java, Go, тощо.), а також командами.
	- зазвичай така проблема вирішується за допомогою перевикористання і створення вхідної точки для створення нового сервісу:
		- зібрані Best practices, що допомагають в розробці production систем, в одному місці. Це необхідно для перевикористання в інших сервісах, що зробить їх production-ready за визначенням, адже ці best practices вже використовуються у всіх production сервісах. Це стосується не лише концептуальних підходів, а й вже готових рішень для перевикористання: конфігурацій метрик, списку залежностей, тощо.
		- Базовий приклад сервісу, що береться за відправну точку для кожного нового сервісу в розподіленій системі. Такий базовий приклад включає в себе всі best practices, які дозволяють вже бути production-ready з першого дня існування.
		- Як наслідок, продуктові інженерні команди можуть більше фокусуватися на продукті, а не на інструментації, моніторингу, чи налаштуванні інфраструктури. 
- софт працює повільніше бо робить екстра роботу
	- деякі розподілені системи можуть сягати тисяч серверів, тому ресурси витрачені на моніторинг та оглядовість таких систем можуть мати великий вплив на продуктивність розподіленої системи. Аби обслуговувати такі розподілені системи, моніторингова система має споживати якомога менше ресурсів, наскільки це можливо, але не менше, аби не спричинити втрати даних або власну недоступність. 
- поміряти скільки по часу це в відсотках займає - наскільки система стає повільнішою.
- деякі сучасні підходи декомпозують це і намагаються робити моніторинг мінімізуючи шкоду системі
	- AOP

Кілька тез у якості висновку:
- Надійність та стабільність систем має багато чого спільного з вимірюванням ризиків і прийманням обдуманих рішень. Аби вимірювати ризики, необхідно розуміти систему, продукт, а також вплив на користувачів. Вимірювання ризиків зазвичай є дорогим інструментом саме по собі.
- Рівень доступності системи має бути підібраний відповідно до того, що бізнес може собі дозволити - з точки зору ціни та впливу помилок на користувачів. Більшість систем не потребують SLA 99.999% та більше.

Table 3. Impact of downtime duration on availability metric.
Downtime per month Downtime per year Availability %
72 hours 36.5 days 90% ("one nine")
7.20 hours 3.65 days 99% ("two nines")
43.8 minutes 8.76 hours 99.9% ("three nines")
4.38 minutes 52.56 minutes 99.99% ("four nines")
25.9 seconds 5.26 minutes 99.999% ("five nines")



##### превентивна безпека за допомогою моніторингу
Ненадійні системи можуть нівельовувати усі позитивні можливості продукту, адже користувачам буде важко ними скористатися. Відповідно, моделюючи систему, ми хочемо мінімізувати ризик проблем з системою, балансуючи з усіма іншими факторами.
Тим не менше, що ми намагаємося запобігти? Говорячи про безпеку, що є небезпекою у цьому контексті?
- незадоволення користувачів
- шкода користувачам або їх власності (напр. даним)
- втрата довіри
- прямі або непрямі втрати прибутків
- негативний вплив на бренд та репутацію
- небажані згадування в засобах масової інформації

Більшість з цих пунктів є прикладними з точки зору бізнесу, адже бізнес є тим на кого будь-що, що стається із системою, має прямий вплив. Так само, більшість з цих пунктів важко вимірювати. 
Врешті-решт, кожен очікує, що система буде працювати стабільно, без втрат, без проблем, і буде приносити дохід компанії, а користувачам цінність. Як каже практика - такого не буває. 

Загальним підходом до проблеми стабільності є моніторинг, який було розглянуто в попередніх розділах. Тим не менше, більшість інструментів в моніторингу сфокусовані на дослідження та вирішення проблеми, коли інцидент відбувається прямо зараз, або вже стався. Напротивагу, ця робота розглядає інциденти задовго до того як вони стаються.

Відповідно, деякі підходи до превентивної безпеки є схожими до підходів вирішення інцидентів в production системах. 

Що можна використати в якості інструментів превентивної безпеки, а також інцидентної безпеки:
- щось

Що явно не підходить:

Моніторинг дає можливість інформувати розробників систем про виявлені негативні тренди в роботі системи, що, у свою чергу, дозволяє їм приймати осмислені рішення для реагування на ці тренди. 
В контексті превентивної безпеки однозначно нема сенсу терміново реагувати на зміни в трендах. Навпаки, є більше сенсу присвятити час аналізу, та лише потім, якщо аналіз показав джерело проблем, то пріорітизувати вирішення таких проблем. 
Так само, задля фокусу на превентивній безпеці, при розробці розподіленої системи необхідно особливу увагу приділяти застереженням. Задля цього можна доповнювати кодову базу різноманітним логуванням, додатковими метриками, що мали б під собою одну мету - визначити гранулярність того, як працює система. Іншими словами, необхідно зрозуміти ситуації, які не є помилкою, проте одночасно й не є happy-path сценарієм.
Найбільше нас цікавлять застереження (warnings) та їх рівень (warnings threshold). 

Базуючись на даних з моніторингу, які презентовані у вигляді дашбордів та трендів, розробники систем та власники продукту можуть робити осмислені рішення, які спровоковані даними. Презентовані метрики на дашбордах можуть допомогти ідентифікувати частини розподіленої системи, які утилізовані більше, аніж це очікувано, або навпаки - утилізовані менше, аніж це очікувано. В обох випадках є спектр доступних рішень, що можна прийняти - чи то масштабування частин системи, переосмислення рішень моделювання системи, відключення частин системи, тощо. 
Demand forecastring -> feedback loop cycle -> upscaling / downscaling / architecture redesign 

Так само, важлива річ, аби згадати, це те, що тренди можуть змінюватись базуючись на змінах в програмному продукті, що ми оглядаємо. Очевидно, що будь-яка зміна в кодовій базі може відобразитися на метриках. Тому, дуже цінним є мати можливість відслідкувати який конкретно артефакт (наприклад Docker образ) продукував такі метрики, аби могти відтворити схожу поведінку вже в іншому середовищі, а також змогти зкорелювати зміну в програмному коді, яка сталась, і зміну в трендах метрик. Чим більше даних ми маємо, аби підтвердити цю кореляцію, тим ліпше, тим змістовнішим є таке рішення.


як моніторинг може запобігти проблемам 

Monitoring progressively delivered systems to prevent

##### превентивна безпека з точки зору менеджера проєкту
- інвестиція, opportunity cost - engineers no longer work on new features. 
- виділити ресурс - людина, група людей - залежно від розміру інвестиції та потреби. 
- SLA як причина інвестиції в превентивну безпеку
- культура та майндсет
Most of the participants believe
that culture and mindset aspects referring to monitoring are essential. Several
even stated that this aspect is more challenging than technical aspects. Furthermore, some interviewees also mentioned that a holistic transparency to apply
monitoring is often not intended. This gives for instance rise to danger of being
blamed in retrospect for a failure. Often, the participants described that teams
do not have an overview outside of their own area, for example of the business
context of their service. This caused isolated monitoring and operation concepts
without context to customer solutions and related requirements. Overall, collaboration and communication between teams and the perspective from which they
develop and operate their services are often weakly pronounced. This illustrates
the following statement (E22): ”It is usually not the ignorance or the inability of
people in the company, but the wrong point of view. Often the developers are so
buried in their problem environment, so engrossed in their daily tasks that they
can no longer afford to change themselves.”

Тому щоб запобігати, є сенс зробити інвестицію в культуру і майндсет.
Monitoring mindset
The prerequisite is to increase the importance
of observability and monitoring of distributed systems. Without an increasing
awareness, isolated ad-hoc solutions will remain, which do not enable sufficient
service provisioning and diagnostics. One mentioned solution to increase the importance of non-functional attributes, like availability and performance, can be
reached by setting and controlling SLOs (S8). Thereby, stability and feature
development can be controlled from management perspective. Further, the participants outlined a need to equalize functional and non-functional requirements.
https://arxiv.org/pdf/1907.12240.pdf


Враховуючи, що ця робота фокусується на моделюванні та інженерній складовій надійності системи, тим не менше, не можна не долучити також менеджерський погляд на цю проблему, а якщо точніше, то погляд того, хто визначає бізнес-цінність продукту. Однією з критично важливих задач власника продукту стосовно надійності є визначити толерантність системи до помилок. Очевидно, що необхідно глибоко розуміти бізнес-цінність продукту та ризики кожної помилки, що може статися. З точки зору власника продукту, необхідно сформулювати бізнес цілі та висловити їх інженерній команді. Особливо це важливо в контексті систем, які мають велику кількість кінцевих користувачів, адже в таких системах є прямий зв'язок між успішністю продукту, та надійністю й продуктивністю системи. 

Намагаючись визначити толерантність системи до помилок, варто задати та знайти відповіді на наступні питання:
- Який рівень доступності є необхідним? 99%, 50%, 99.999%? 
- Як правильно виміряти рівень доступності для даної системи? Чи є це зкорельованим із кількістю успішно виконаних запитів, чи радше з часом, коли система працює і відповідає хоч якось на запити?
- Чи можна виділити типи помилок, де деякі є більш критичними для продукту за інші?
- Які метрики варто враховувати досліджуючи поведінку цієї системи?
- На що впливають помилки з цієї системи? Який ризик? Що під цим ризиком - наші дані, дані користувачів, прибуток, прибуток користувачів, тощо?
- Якщо є схожі системи доступні на ринку, то які рівні доступності вони показують?
- Якщо проблема стається, то що ліпше - не приймати зовсім ніякі запити, або приймати, але обслуговувати частину з них?

##### Postmortem культура як частина превентивної безпеки
Коригуючі та превентивні дії (Corrective and Predictive Actions) - це загально-відома концепція для покращення надійності, яка фокусується на систематичному дослідженні причин ідентифікованих проблем та ризиків задля того, аби запобігти їхньому повторенню.
Одним з таких CAPA інструментів в моніторингу розподілених систем є postmortem культура.

Що таке Postmortem, як це культура.
465...

В контексті превентивної безпеки, в кожному postmortem дуже важливо отримувати відповіді на запитання:
- що можна було б зробити інакше наступного разу, якщо це трапиться?
- що можна зробити, аби цього не трапилось знову?

Ці відповіді обов'язково мають бути досліджені не лише в рамках команди, яка опрацювала інцидент, а й серед інших інженерних команд, аби всі мали можливість отримати розуміння того, що сталось. Таким чином, знання поширюватимуться по організації та запобігатимуть появі нових інцидентів.



# економічна частина

# висновки

Моніторинг - це незамінна, необхідна частина сучасних розподілених систем для візуалізації, тестування, відлагодження та розробки. Це незамінний інструмент роботи з production системами. 

Ця робота презентувала загальні підходи до моніторингу із наголосом на превентивну безпеку, описуючи основні підходи до превентивної безпеки. Ось ці підходи:
- пост-мортем
- аналіз довготривалих трендів і вміння зкорелювати тренди із змінами в кодовій базі
- здоровий менеджерський погляд на превентивну безпеку та стабільність розподіленої системи

- що вдалось запобігти
- як це впливає на бізнес

In this guide, we’ve talked about some of the specific challenges that distributed architectures and microservice designs can introduce for monitoring and visibility software. Modern ways of building systems break some assumptions of traditional methods, requiring different approaches to handle the new configuration environments. We explored the adjustments you’ll need to consider as you move from monolithic systems to those that increasingly depend on ephemeral, cloud or container-based workers and high volume network coordination. Afterwards, we discussed some ways that your system architecture might affect the way you respond to incidents and resolution.

We identified that monitoring and the observability of distributed systems is not purely a technical issue anymore but becomes
a more cross-cutting and strategic topic, critical to the success of a company
which offers services
рекомендації щодо використання результатів бакалаврської роботи в навчальному процесі університету.
зв’язок виконаної роботи з науково-дослідними розробками кафедри САП;

## Майбутні роботи
- що можна далі зробити

Дослідити роль AI в рамках predictive prevention, anomaly detection for preventive safety based on monitoring. 
s. The most
advanced method for anomaly detection is AI (S14). Almost all participants appreciated its enormous potential to master the complexity and the flood of data
generated by distributed systems. However, many interviewees pointed out that
sufficient preconditions for the use of AI are still missing in practice. Primarily,
the right data has to be collected, the quality of data has to be ensured, the
context needs to be propagated and data has to be stored centrally. Concerns in
terms of the cost value ratio of AI approaches and their reliability remain
# список використаних джерел

Robert Cooper and Keith Marzullo. Consistent detection of global predicates. In Proc. of the ACM/ONR Workshop on Parallel and Distributed Debugging, pages 163–173, 1991.

On Observability and Monitoring of Distributed
Systems – An Industry Interview Study 29 Jul 2019 https://arxiv.org/abs/1907.12240
Sina Niedermaier Falko Koetter Andreas Freymann and Stefan Wagner


sre book

# додатки

# ACKNOWLEDGMENTS

We would like to thank the faculty, students, and staff associated with the Jade
Project for contributing ideas and for providing an excellent environment in
which to explore these ideas. Special recognition is due John Cleary and Radford
Neal for their contributions to this work. We would also like to express our
appreciation to the Natural Science and Engineering Research Council of Canada
for supporting this research. 