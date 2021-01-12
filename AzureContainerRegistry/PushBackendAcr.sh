REGISTRY_NAME="efurni"
USER_NAME="efurni"
PASSWORD=""
PARENT_DIR="$(dirname "$(pwd)")"

docker build -t efurni-core -f ../DockerizeCore.Dockerfile "$PARENT_DIR"

docker tag efurni-core efurni.azurecr.io/efurni-core

az acr login --name $REGISTRY_NAME --username $USER_NAME --password $PASSWORD
if [ $? -ne 0 ]; then
  exit 0
fi

az acr repository delete --name efurni --repository efurni-core --yes

docker push efurni.azurecr.io/efurni-core