echo "===== DockerBuild API version $1 ====="
docker build -f ./OSCiR/Dockerfile -t oscir_api:$1 . 
