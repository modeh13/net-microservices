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

Starts Kubernetes for Shopping Cart and list nodes.
```bash
kubectl apply -f shopping-cart.yaml
kubectl get all
```

Stop and Delete Kubernetes Cluster
```bash
kubectl delete -f shopping-cart.yaml
```

### Kubernetes Dashboard
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.7.0/aio/deploy/recommended.yaml
kubectl proxy
kubectl -n kube-system describe secret default
kubectl apply -f dashboard-adminuser.yaml
kubectl -n kubernetes-dashboard create token admin-user
```