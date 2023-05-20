# Shopping Cart Microservice

## Docker

Builds and creates a Docker Image which contains the Microservice
```bash
docker build . -t shopping-cart
````

Uses Docker Image to create a Container
```bash
docker run --name [Container-Name] --rm -p 5001:60737 [DockerImage-Name]
docker run --name shopping-cart --rm -p 5001:60737 shopping-cart
```

## Kubernetes

Checks Kubernetes is up and running.
```bash
kubetctl cluster-info
```

Starts Kubernetes for Shopping Cart and list nodes.
```bash
kubectl apply -f shopping-cart.yaml
kubectl get all
```

Stop and Delete Kubernetes Cluster
```bash
kubectl delete -f shopping-cart.yaml
```

Switch Contexts
```bash
kubectl config get-contexts
kubectl config use-context docker-desktop
```

### Kubernetes Dashboard
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml
kubectl proxy
kubectl -n kube-system describe secret default
```

#### Access Dashboard

```text
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/
```

Create Service Account following manifest file:
```bash
kubectl apply -f dashboard-adminuser.yaml
kubectl apply -f dashboard-cluster-role-binding.yaml
```

Get Bearer Token:
```bash
kubectl -n kubernetes-dashboard create token admin-user
```

Remove Services Account and ClusterRoleBinding
```bash
kubectl -n kubernetes-dashboard delete serviceaccount admin-user
kubectl -n kubernetes-dashboard delete clusterrolebinding admin-user
```

### AKS Kubernetes Dashboard

```bash
kubectl create clusterrolebinding kubernetes-dashboard --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard
```

### Push Docker Image to Azure Container Registry

Tag Docker Image. This is going to create a new Image referencing our Container Registry and version.

```bash
docker tag shopping-cart grcontainerreg.azurecr.io/shopping-cart:1.0.0
az acr login --name grcontainerreg
docker push grcontainerreg.azurecr.io/shopping-cart:1.0.0
```

## Data store

Pull Microsoft SQL Server Docker image:
```batch
docker pull mcr.microsoft.com/mssql/server 
``` 

Run Docker container for SQL Server:
```batch
docker run --name ms-sql-server -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=123Test!" -p 1433:1433 -d mcr.microsoft.com/mssql/server
``` 