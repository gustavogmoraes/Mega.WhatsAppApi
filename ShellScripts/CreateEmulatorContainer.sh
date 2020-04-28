docker run --privileged -dit --name android-container ubuntu;

docker exec -it android-container bin/bash;

apt update && apt install -y openjdk-8-jdk vim git unzip libglu1 libpulse-dev libasound2 libc6  libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxi6  libxtst6 libnss3 wget

wget https://services.gradle.org/distributions/gradle-5.4.1-bin.zip -P /tmp \
&& unzip -d /opt/gradle /tmp/gradle-5.4.1-bin.zip

mkdir /opt/gradlew \
&& /opt/gradle/gradle-5.4.1/bin/gradle wrapper --gradle-version 5.4.1 --distribution-type all -p /opt/gradlew \
&& /opt/gradle/gradle-5.4.1/bin/gradle wrapper -p /opt/gradlew

wget 'https://dl.google.com/android/repository/sdk-tools-linux-4333796.zip' -P /tmp \
&& unzip -d /opt/android /tmp/sdk-tools-linux-4333796.zip \

touch /usr/local/share/android-sdk

yes Y | /opt/android/tools/bin/sdkmanager --install "platform-tools" "system-images;android-28;google_apis;x86" "platforms;android-28" "build-tools;28.0.3" "emulator"

yes Y | /opt/android/tools/bin/sdkmanager --licenses

echo "no" | /opt/android/tools/bin/avdmanager --verbose create avd --force --name "test" --device "pixel" --package "system-images;android-28;google_apis;x86" --tag "google_apis" --abi "x86"

vim ~/.bashrc

GRADLE_HOME=/opt/gradle/gradle-5.4.1/
ANDROID_HOME=/opt/android/
PATH=$PATH:$GRADLE_HOME/bin/:/opt/gradlew:$ANDROID_HOME/emulator:$ANDROID_HOME/tools/bin/:$ANDROID_HOME/platform-tools/
LD_LIBRARY_PATH=$ANDROID_HOME/emulator/lib64/:$ANDROID_HOME/emulator/lib64/qt/lib/

source ~/.bashrc

sudo apt-get install qemu-kvm libvirt-bin ubuntu-vm-builder bridge-utils

