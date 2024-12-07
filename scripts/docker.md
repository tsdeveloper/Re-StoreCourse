```bash
docker network create -d bridge localdev 
docker compose down && docker rm -f api sqlserver && docker compose up -d
docker system prune -a -f
docker volume prune -a -f
docker volume rm $(docker volume ls -q --filter dangling=true)
docker volume ls -qf dangling=true | xargs -r docker volume rm
docker stop $(docker ps -aq)
docker container rm -f $(docker container ls -aq)
docker volume rm -f $(docker volume ls -q)
```
## Step 1: Stopping containers
+ To delete all volumes, one first has to stop all containers. Ref this answer, this can be done like so:
```bash
docker stop $(docker ps -aq)
```
+ ...and verified with this command:
```bash
docker ps
```
+ ...if nothing shows up in the list, then you've stopped all containers.

+ The reason this is required is because Docker does not allow you to remove volumes for containers that are currently running - even with --force.


## Step 2: Deleting all containers
+ Next, we need to remove all containers:
```bash
docker container rm -f $(docker container ls -aq)
```
+ Note that containers and images are 2 different things. Containers are instances of a given image with e.g. volumes etc associated with them. Even if you stop containers from running, they are not automatically deleted.

+ Step 3: Deleting all volumes
+ Now that all containers are stopped and deleted, we can remove all volumes like so:
```bash
docker volume rm -f $(docker volume ls -q)
```