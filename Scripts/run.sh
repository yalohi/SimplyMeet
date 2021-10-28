#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
. ${SCRIPT_PATH}/env.sh

if [ -z "${MY_BUILD_PATH}" ]; then exit 1; fi

cd ${SCRIPT_PATH}/../${MY_BUILD_PATH}
docker-compose up