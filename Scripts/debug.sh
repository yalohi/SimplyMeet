#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
. ${SCRIPT_PATH}/env.sh

cd ${SCRIPT_PATH}
./build.sh debug || exit 1
./run.sh || exit 1