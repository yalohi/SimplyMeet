#!/bin/bash
set -euo pipefail

readonly SCRIPT_DIR="$(cd "$(dirname "${0}")"; pwd -P)"
readonly SM_MAIN_DIR="${SCRIPT_DIR}/.."

readonly SM_BUILD_CONFIG="${1:-"release"}"
readonly SM_BUILD_ARCH="${2:-"linux-musl-x64"}"

readonly SM_BUILD_DIR="Build"
readonly SM_WASM_PROJECT="${SM_MAIN_DIR}/SimplyMeetWasm/SimplyMeetWasm.csproj"
readonly SM_WEBAPI_PROJECT="${SM_MAIN_DIR}/SimplyMeetApi/SimplyMeetApi.csproj"
readonly SM_WASM_BUILD_DIR="${SM_BUILD_DIR}/SimplyMeetWasm"
readonly SM_WEBAPI_BUILD_DIR="${SM_BUILD_DIR}/SimplyMeetApi"

cd "${SM_MAIN_DIR}"

if [ "${SM_BUILD_CONFIG^^}" = "DEBUG" ]; then
	[ -f "${SM_WASM_PROJECT}" ] && \
		dotnet publish "${SM_WASM_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WASM_BUILD_DIR}" \
			-r "${SM_BUILD_ARCH}" \
			-p:SelfContained=True \
			-p:PublishTrimmed=False \
			-p:BlazorEnableCompression=False

	[ -f "${SM_WEBAPI_PROJECT}" ] && \
		dotnet publish "${SM_WEBAPI_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WEBAPI_BUILD_DIR}" \
			-r "${SM_BUILD_ARCH}" \
			-p:SelfContained=True \
			-p:PublishReadyToRun=False \
			-p:PublishSingleFile=True \
			-p:PublishTrimmed=False \
			-p:IncludeNativeLibrariesForSelfExtract=True
elif [ "${SM_BUILD_CONFIG^^}" = "RELEASE" ]; then
	[ -f "${SM_WASM_PROJECT}" ] && \
		dotnet publish "${SM_WASM_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WASM_BUILD_DIR}" \
			-r "${SM_BUILD_ARCH}" \
			-p:SelfContained=True \
			-p:PublishTrimmed=True \
			-p:BlazorEnableCompression=True

	[ -f "${SM_WEBAPI_PROJECT}" ] && \
		dotnet publish "${SM_WEBAPI_PROJECT}" \
			-c "${SM_BUILD_CONFIG}" \
			-o "${SM_WEBAPI_BUILD_DIR}" \
			-r "${SM_BUILD_ARCH}" \
			-p:SelfContained=True \
			-p:PublishReadyToRun=False \
			-p:PublishSingleFile=True \
			-p:PublishTrimmed=True \
			-p:IncludeNativeLibrariesForSelfExtract=True

	rm -f \
		"${SM_WASM_BUILD_DIR}/"*".pdb" \
		"${SM_WEBAPI_BUILD_DIR}/"*".pdb"
fi
