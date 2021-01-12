kubectl delete deployment/redis-deployment
kubectl delete service/redis-cluster

kubectl apply -f Deployments/redis-deployment.yml
kubectl apply -f Services/redis-cluster.yml
