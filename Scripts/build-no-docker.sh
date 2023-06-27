#!/bin/bash

SCRIPT_PATH=$(dirname $(realpath "$0"))
MY_BUILD_CONFIG=${1}
MY_BUILD_ARCH=${2}
. ${SCRIPT_PATH}/env.sh

if [ $# -le 0 ]; then
	MY_BUILD_CONFIG=release
fi

if [ $# -le 1 ]; then
	MY_BUILD_ARCH=linux-musl-x64
fi

if [ -z "${MY_BUILD_PATH}" ]; then exit 1; fi

cd ${SCRIPT_PATH}/../

if [ "${MY_BUILD_CONFIG^^}" = "DEBUG" ]; then
	dotnet publish SimplyMeetWasm/SimplyMeetWasm.csproj -c ${MY_BUILD_CONFIG} -o ${MY_BUILD_PATH}/wasm/app -r ${MY_BUILD_ARCH} -p:SelfContained=True -p:PublishTrimmed=False -p:BlazorEnableCompression=False
	dotnet publish SimplyMeetApi/SimplyMeetApi.csproj -c ${MY_BUILD_CONFIG} -o ${MY_BUILD_PATH}/webapi/app -r ${MY_BUILD_ARCH} -p:SelfContained=True -p:PublishReadyToRun=False -p:PublishSingleFile=True -p:PublishTrimmed=False -p:IncludeNativeLibrariesForSelfExtract=True
elif [ "${MY_BUILD_CONFIG^^}" = "RELEASE" ]; then
	dotnet publish SimplyMeetWasm/SimplyMeetWasm.csproj -c ${MY_BUILD_CONFIG} -o ${MY_BUILD_PATH}/wasm/app -r ${MY_BUILD_ARCH} -p:SelfContained=True -p:PublishTrimmed=True -p:BlazorEnableCompression=True
	dotnet publish SimplyMeetApi/SimplyMeetApi.csproj -c ${MY_BUILD_CONFIG} -o ${MY_BUILD_PATH}/webapi/app -r ${MY_BUILD_ARCH} -p:SelfContained=True -p:PublishReadyToRun=False -p:PublishSingleFile=True -p:PublishTrimmed=True -p:IncludeNativeLibrariesForSelfExtract=True
	rm -f ${MY_BUILD_PATH}/wasm/app/*.pdb ${MY_BUILD_PATH}/webapi/app/*.pdb
fi