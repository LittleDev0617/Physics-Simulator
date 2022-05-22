using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Pendulum : MonoBehaviour
{
    private bool isVelocity = true;
    private int mass = 1;
    private double length = 0.24f; //meter
    private double start_theta = 30 * Math.PI / 180;
    private double g = 9.80665d;
    private double theta;


    private bool isToggle = false;
    IEnumerator tick()
    {
        double t = 0f;
        double dt = Time.deltaTime;
        double v = 0;
        bool tmp = true;
        int tmp2 = 1;
        Text timeText = GameObject.Find("Time").GetComponent<Text>();
        while(true)
        {
            if(isVelocity) 
            {
                theta += V_theta(theta) * dt;
            }
            else
            {
                v += A_theta(theta) * dt;
                theta += v * dt;
            }

           //Debug.Log($"[{t}] theta : {theta * 180 / Math.PI} v : {v} - {isToggle}");
            if(tmp == isToggle)
            {
                if(tmp2 % 2 == 0) GameObject.Find("MagentaT").GetComponent<Text>().text = $"Time : {Math.Round(t,4)} s";
                tmp = !tmp;
                tmp2++;
            }

            transform.eulerAngles = new Vector3(0,0,(float)(theta * 180 / Math.PI));

            t += dt;
            timeText.text = $"t = {Math.Round(t,4)}";
            yield return new WaitForSecondsRealtime((float)dt);
        }
    }

    void Play()
    {
        StopCoroutine("tick");

        double T = GetT();
        GameObject.Find("T").GetComponent<Text>().text = Math.Round(T,4).ToString() + " s";
        theta = start_theta - 0.00000000001d;
        transform.eulerAngles = new Vector3(0,0,(float)(start_theta * 180 / Math.PI));
        StartCoroutine("tick");
    }

    IEnumerator Rainbow()
    {
        while(true)
        {
            equation.GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0,255) /255f,UnityEngine.Random.Range(0,255)/255f,UnityEngine.Random.Range(0,255)/255f);                
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Start()
    {
        Play();
        equation = GameObject.Find("Equation");
    }

    private double V_theta(double theta)
    {
        if(Math.Cos(theta) - Math.Cos(start_theta) <= 0) isToggle = !isToggle;
        return  System.Math.Sqrt(2*g*length*(Math.Abs(Math.Cos(theta) - Math.Cos(start_theta)))) / length * (isToggle ? 1 : -1);
    }

    private double A_theta(double theta)
    {
        if(isToggle == (Math.Sin(theta) > 0)) isToggle = !isToggle;
        return -1 * g * Math.Sin(theta) / length;
    }


    public void GoButton()
    {
        Text angle = GameObject.Find("Angle").transform.GetChild(2).GetComponent<Text>();
        Text length = GameObject.Find("Length").transform.GetChild(2).GetComponent<Text>();

        double a;
        double b;
        if((angle.text.Trim() == "" || !double.TryParse(angle.text,NumberStyles.Number,CultureInfo.InvariantCulture,out a)) ||
            (length.text.Trim() == "" || !double.TryParse(length.text,NumberStyles.Number,CultureInfo.InvariantCulture,out b)))   return;

        if(a == 0 || b == 0) return;
        start_theta = a * Math.PI / 180;
        this.length = b;

        GameObject.Find("MagentaP").GetComponent<Text>().text = $"Angle   : { Math.Round(this.start_theta * 180 / Math.PI,2) }º\nLength : { Math.Round(this.length,2) }m";
        GameObject.Find("MagentaT").GetComponent<Text>().text = $"Time : 0 s";
        Play();
    }

    private double GetT()
    {
        double sum = 1;
        for(int i=1;i<=32;i++)
            sum += Math.Pow(DoubleFactorial(2*i - 1) * 1.0d / DoubleFactorial(2*i),2) * Math.Pow(Math.Sin(start_theta / 2),2*i);
        
        return 2 * Math.PI * Math.Sqrt(length/g) * sum;
    }

    private ulong DoubleFactorial(int x)
    {
        if (x < 1) return 1;
        int n = x;
        ulong y = 1;
        while (n > 0) {
            y *= (ulong)n;
            n -= 2;
        }
        return y;
    }

    private bool isRainbow = false;
    private GameObject equation;
    public void RainbowT()
    {
        StopCoroutine("Rainbow");
        isRainbow = !isRainbow;
        if(isRainbow) StartCoroutine("Rainbow");
        else equation.GetComponent<Image>().color = new Color(255,0,220);
    }

    public GameObject HelpObject;
    private bool helpToggle = false;
    public void HelpButton()
    {
        HelpObject.SetActive(!HelpObject.active);
    }
}