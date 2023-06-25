#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
. ${SCRIPT_PATH}/env.sh

if [ -z "${MY_BACKUPS_PATH}" ]; then exit 1; fi
if [ -z "${MY_BACKUP_NAME}" ]; then exit 1; fi
if [ -z "${MY_BACKUP_DAYS}" ]; then exit 1; fi
if [ -z "${MY_CONTAINER_NAME_WEBAPI}" ]; then exit 1; fi
if [ -z "${MY_TEMP_BACKUP_PATH}" ]; then exit 1; fi
if [ -z "${MY_DATA_PATH}" ]; then exit 1; fi

cd ${SCRIPT_PATH}/../

mkdir -p ${MY_BACKUPS_PATH}
cd ${MY_BACKUPS_PATH}

CONTAINER_WAS_RUNNING=$(docker ps|grep ${MY_CONTAINER_NAME_WEBAPI})

docker stop ${MY_CONTAINER_NAME_WEBAPI} || exit 1
docker run --rm --volumes-from ${MY_CONTAINER_NAME_WEBAPI} -v $(pwd):${MY_TEMP_BACKUP_PATH} alpine tar cvf /backup/${MY_BACKUP_NAME} ${MY_DATA_PATH} || exit 1

if [ "${CONTAINER_WAS_RUNNING}" ]; then
	docker start ${MY_CONTAINER_NAME_WEBAPI}
fi

find ./*.tar -maxdepth 0 -type f -mtime +${MY_BACKUP_DAYS} -exec rm -rf {} +
echo "Created Backup: ${MY_BACKUP_NAME}"