#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
BACKUP_NUM=${1}
. ${SCRIPT_PATH}/env.sh

if [ $# -le 0 ]; then
	BACKUP_NUM=1
fi

if [ -z "${MY_BACKUPS_PATH}" ]; then exit 1; fi
if [ -z "${MY_CONTAINER_NAME_WEBAPI}" ]; then exit 1; fi
if [ -z "${MY_TEMP_BACKUP_PATH}" ]; then exit 1; fi
if [ -z "${MY_DATA_PATH}" ]; then exit 1; fi

cd ${SCRIPT_PATH}/../

mkdir -p ${MY_BACKUPS_PATH}
cd ${MY_BACKUPS_PATH}

CONTAINER_WAS_RUNNING=$(docker ps|grep ${MY_CONTAINER_NAME_WEBAPI})
BACKUP_RESTORE_NAME=$(ls -t|head -n${BACKUP_NUM}|tail -n1)

docker stop ${MY_CONTAINER_NAME_WEBAPI} || exit 1
docker run --rm --volumes-from ${MY_CONTAINER_NAME_WEBAPI} -v $(pwd):${MY_TEMP_BACKUP_PATH} alpine /bin/sh -c "cd ${MY_DATA_PATH} && rm -rf * && tar xvf ${MY_TEMP_BACKUP_PATH}/${BACKUP_RESTORE_NAME} --strip 2" || exit 1

if [ "${CONTAINER_WAS_RUNNING}" ]; then
	docker start ${MY_CONTAINER_NAME_WEBAPI}
fi

echo "Restored Backup: ${BACKUP_RESTORE_NAME}"