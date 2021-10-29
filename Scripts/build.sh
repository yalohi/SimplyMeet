#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
. ${SCRIPT_PATH}/env.sh

if [ -z "${MY_DOCKER_PATH}" ]; then exit 1; fi
if [ -z "${MY_BUILD_PATH}" ]; then exit 1; fi
if [ -z "${MY_VOLUME_NAME_WASM_APP}" ]; then exit 1; fi
if [ -z "${MY_VOLUME_NAME_WEBAPI_APP}" ]; then exit 1; fi

cd ${SCRIPT_PATH}/../${MY_DOCKER_PATH}
docker-compose rm -f || exit 1
docker volume rm -f ${MY_VOLUME_NAME_WASM_APP} || exit 1
docker volume rm -f ${MY_VOLUME_NAME_WEBAPI_APP} || exit 1

cd ${SCRIPT_PATH}/../
rsync -a --delete ${MY_DOCKER_PATH} ${MY_BUILD_PATH} || exit 1

cd ${SCRIPT_PATH}
./build-no-docker.sh ${1} ${2} || exit 1

cd ${SCRIPT_PATH}/../${MY_BUILD_PATH}
docker-compose build || exit 1
docker system prune -f || exit 1