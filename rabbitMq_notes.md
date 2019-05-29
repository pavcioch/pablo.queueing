Install:
1. /install/otp_win64_21.3.exe - run as administrator, must be installed to default path C:\Program Files\
2. RUN: set ERLANG_HOME=C:\Program Files\erl10.3
3. /install/rabbitmq-server-3.7.14.exe - run as administarator
4. enable ManagementUI plugin:
   RUN: C:\Program Files\RabbitMQ Server\rabbitmq_server-3.7.15\sbin> rabbitmq-plugins enable rabbitmq_management

Defaults:
rabbitMQ server: http://localhost:15672
user: guest
pwd: guest


Docs
- official tutorials: https://www.rabbitmq.com/tutorials/
- tutorials source: https://github.com/rabbitmq/rabbitmq-tutorials/tree/master/dotnet
- management Http Api: https://rawcdn.githack.com/rabbitmq/rabbitmq-management/v3.7.15/priv/www/api/index.html 

Types of exchange:
- default : exchange = "", routingKey=<queue_name>
- direct : exchange = <exchange_name>, binding by [queue_name, exchange_name, routingKey], BasicPublish: use exchange_name && routingKey, good for load balancing
- fanout : exchange = <exchange_name>, binding by [queue_name, exchange_name], BasicPublish: use exchange_name
- topic : 
- headers





