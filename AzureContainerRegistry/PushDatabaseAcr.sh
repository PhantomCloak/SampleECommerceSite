REGISTRY_NAME="efurni"
USER_NAME="efurni"
PASSWORD=""
PARENT_DIR="$(dirname "$(pwd)")"

docker build -t postgres -f ../DockerizePostgres.Dockerfile $PARENT_DIR

docker tag postgres:latest efurni.azurecr.io/postgres

az acr login --name $REGISTRY_NAME --username $USER_NAME --password $PASSWORD
if [ $? -ne 0 ]; then
  exit 0
fi

az acr repository delete --name efurni --repository postgres --yes

docker push efurni.azurecr.io/postgres