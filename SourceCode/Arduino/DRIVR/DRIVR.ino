#include<Wire.h>

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

//variable declarations
int IRSensor = 4;
int buttonPin = 5;
float length1 = 0.282;
float speed1 = 0.0;

bool readyStart = true;
bool readyStop = false;
bool earlyDetect = false;

unsigned long startMicros = 0;
unsigned long stopMicros = 0;
unsigned long elapsedMicros = 0;
bool initialReadingTaken = false;

float initialAngle = 0.0;
float hitAngle = 0.0;
float movedAngle = 0.0;

const int MPU_addr = 0x68;
int16_t AcX, AcY, AcZ, Tmp, GyX, GyY, GyZ;

int minVal = 265;
int maxVal = 402;

double x;
double y;
double z;

void setup()
{
  //setup of IRSensor and button
  pinMode (IRSensor, INPUT);
  pinMode(buttonPin, INPUT_PULLUP);
  
  //MPU6050 gyroscope code adapted from here
  //https://www.instructables.com/How-to-Measure-Angle-With-MPU-6050GY-521/
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);

  //begins serial communication at specified baud rate
  Serial.begin(115200);
  Serial.println();
}

void loop()
{
  //button and IRSensor values read in
  int statusSensor = digitalRead (IRSensor);
  int buttonValue = digitalRead(buttonPin);

  //MPU6050 gyroscope code adapted from here
  //https://www.instructables.com/How-to-Measure-Angle-With-MPU-6050GY-521/
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);
  Wire.endTransmission(false);

  Wire.requestFrom(MPU_addr, 14, true);
  AcX = Wire.read() << 8 | Wire.read();
  AcY = Wire.read() << 8 | Wire.read();
  AcZ = Wire.read() << 8 | Wire.read();

  int xAng = map(AcX, minVal, maxVal, -90, 90);
  int yAng = map(AcY, minVal, maxVal, -90, 90);
  int zAng = map(AcZ, minVal, maxVal, -90, 90);

  x = RAD_TO_DEG * (atan2(-yAng, -zAng) + PI);
  y = RAD_TO_DEG * (atan2(-xAng, -zAng) + PI);
  z = RAD_TO_DEG * (atan2(-yAng, -xAng) + PI);

  //if the button is pressed, reset everything
  //read in initial angle
  if (buttonValue == LOW) {

    readyStart = true;
    readyStop = false;
    earlyDetect = false;

    startMicros = 0;
    stopMicros = 0;
    elapsedMicros = 0;

    initialAngle = y;
    Serial.print("initial Angle : ");
    Serial.println(initialAngle);
    
    hitAngle = 0.0;
    movedAngle = 0.0;

    Serial.println("Button Pressed");
  }

  //if it doesn't sense the object
  if (statusSensor == 1)
  {
    //once the club's backkswing has left the box, then get ready to read in time
    if (earlyDetect == false)
    {
      Serial.println("early detecting...finished!");
      earlyDetect = true;
    }
    
    //if the club has pased the box on the swing, then stop the timer, calculate the speed and output
    //also calculates the angle and outputs
    if (readyStop == true && earlyDetect == true)
    {
      stopMicros = micros();
      readyStop = false;

      elapsedMicros = stopMicros - startMicros;

      speed1 = ((length1 / elapsedMicros) * 1000000) * 2.237;

      Serial.print("Speed(mph) : ");
      Serial.println(speed1);

      hitAngle = y;
      movedAngle = hitAngle - initialAngle;

      Serial.print("Moved Angle : ");
      Serial.println(movedAngle);
    }
  }
  //if it senses the object
  else if (statusSensor == 0)
  {
    //if it is an early detect then ignore
    if (earlyDetect == false)
    {
      Serial.println("detecting...early detect");
    }

    //if the club is ready, begin the timer
    if (readyStart == true && earlyDetect == true)
    {
      startMicros = micros();
      readyStop = true;
      readyStart = false;
    }
  }
}
