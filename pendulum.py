from math import *

#https://javalab.org/period_of_pendulum/
dt = 0.001
theta_max = 30 * pi / 180;
theta_start = theta_max 
theta = theta_start- 0.000000001
l = 0.24
g = 9.80665
t = 0

isToggle = False
print(pi * sqrt(l/g))

while True:
    theta += sqrt(2*g*l*(abs(cos(theta) - cos(theta_max)))) * dt / l * (1 if isToggle else -1)
    t += dt
    if cos(theta) - cos(theta_max) <= 0:
        break
        

    print('[%f] : %f' % (t,theta * 180 / pi))

print(2 * t)