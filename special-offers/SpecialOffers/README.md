# Special Offers

Build Docker Image

```batch
docker build ".\SpecialOffers" -t special-offers
```

Uses Docker Image to create a Container

```bash
docker run --name special-offers --rm -p 8082:80 special-offers
```