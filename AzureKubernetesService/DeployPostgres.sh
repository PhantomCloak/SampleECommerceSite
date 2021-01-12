# known issues if pod deploys while old ones terminating it causes volume error 
( exec ../AzureContainerRegistry/PushDatabaseAcr.sh)

kubectl delete deployment/postgresql-deployment
kubectl delete service/database-cluster

kubectl apply -f Deployments/postgres-deployment.yml
kubectl apply -f Services/database-cluster.yml

kubectl get all