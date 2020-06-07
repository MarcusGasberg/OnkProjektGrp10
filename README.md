## How to run the app

Start minikube and get ip:
``` cmd
>>> minikube start
>>> minikuibe ip
192.168.99.100
```
Add to the bottom of /etc/host the ip of minikube and then the following urls:
```
192.168.99.100  web-app.stocks
192.168.99.100  identity-server.stocks
```
Make Minikube your docker enviroment and enable ingress:
``` 
minikube -p minikube docker-env | Invoke-Expression
minikube addons enable ingress
```

Create docker images:
```
../src/Services>>> docker-compose build
```
Apply the kubernetes configuration to minikube:
```
../src/Services>>> kubectl apply -f ./kubernetes
```
Wait for the services to spin up and navigate to:

[web-app.stocks](http://web-app.stocks)