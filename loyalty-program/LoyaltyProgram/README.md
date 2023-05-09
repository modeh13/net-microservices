# Loyalty Program Service

Build Docker Image

```batch
docker build ".\LoyaltyProgram" -t loyalty-program
```

Create Docker Container
```batch
docker run --name loyalty-program --rm -p 8083:80 loyalty-program
```