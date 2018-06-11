FROM alpine:3.7

RUN apk -U --no-cache add ca-certificates python3 python3-dev build-base libffi-dev openssl-dev && \
    python3 -m ensurepip && \
    pip3 install --upgrade pip setuptools && \
    pip3 install locustio pyzmq adal Faker applicationinsights && \
    apk del python3-dev && \
    mkdir /locust

WORKDIR /locust

ONBUILD ADD . /locust

EXPOSE 8089 5557 5558
ENTRYPOINT [ "locust" ]
