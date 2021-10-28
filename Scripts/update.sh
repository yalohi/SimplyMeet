#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
. ${SCRIPT_PATH}/env.sh

cd ${SCRIPT_PATH}
./backup-create.sh || exit 1
git pull || exit 1
./build.sh || exit 1