REGISTRY_NAME="efurni"
USER_NAME="efurni"
PASSWORD=""
PARENT_DIR="$(dirname "$(pwd)")"

docker build -t efurni-presentation -f ../DockerizePresentation.Dockerfile "$PARENT_DIR"

docker tag efurni-presentation efurni.azurecr.io/efurni-presentation

az acr login --name $REGISTRY_NAME --username $USER_NAME --password $PASSWORD
if [ $? -ne 0 ]; then
  exit 0
fi

az acr repository delete --name efurni --repository efurni-presentation --yes

docker push efurni.azurecr.io/efurni-presentation