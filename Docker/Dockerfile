FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder
WORKDIR /source
RUN apk add --no-cache --update bash git
RUN git clone --depth 1 --branch dev "https://github.com/yalohi/SimplyMeet" .
RUN ./Scripts/build-no-docker.sh

FROM docker.io/nginx:mainline-alpine
WORKDIR /etc/nginx/html
COPY --from=builder /source/Docker/nginx-wasm.conf /etc/nginx/nginx.conf
COPY --from=builder /source/Build/SimplyMeetWasm/wwwroot .

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-alpine
WORKDIR /app
COPY --from=builder /source/Build/SimplyMeetApi .
RUN apk add --no-cache --update sqlite
ENTRYPOINT [ "./SimplyMeetApi" ]
