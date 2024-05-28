[![GitHub license](https://img.shields.io/github/license/cartheur/aeon)](https://github.com/cartheur/aeon/blob/main/LICENSE)
[![Build status](https://github.com/github/cartheur/actions/workflows/dotnet-desktop.yml)
[![GitHub issues](https://img.shields.io/github/issues/cartheur/aeon)](https://github.com/cartheur/aeon/issues)

# aeon
An artificial general intelligence (AGI) program

What is AGI?

From Wikipedia:

Artificial general intelligence (AGI) is the ability of an intelligent agent to understand or learn any intellectual task that human beings or other animals can. It is a primary goal of some artificial intelligence research and a common topic in science fiction and futures studies. AGI is also called strong AI, full AI, or general intelligent action, although some academic sources reserve the term "strong AI" for computer programs that experience sentience or consciousness.

## What is _an_ AGI?

Artificial general intelligence (AGI) is the ability of an intelligent agent to understand or learn any intellectual task that human beings or other animals can.

## Why _this_ AGI?

Despite the noise from billionaires and influencers, a long and durative education to the highest levels is requisite to gather the correct "six fundamental knowledge pivots" needed to begin with an architectural and runtime design. This was first published in United States Patent Application US20180204107A1 in July 2018.

As the marketing has ramped-up to ride the hype generated by ChatGPT, that it is some kind of AGI (it is not), this project is that very thing.

## Paitence is warranted

Although having been in existence for quite a few years, _this_ AGI is under development yet updated frequently. A public announcement will be made when it is ready for release. We are getting there slowly. This code is fairly antiquated, from about May 2016, when this was the order of the day:

![meeting-of-minds](/media/meeting-of-the-minds.png)

## How to use this AGI

Clone the repo and run the Aeon.Runtime console application. Ask questions if stuck.

## Updates

23.03.2023: I have uploaded the compendium of the _Mindpixel_ database as I rendered it in XML-format. Added documentation.
09.08.2023: I have updated aspects of the runtime to optimize its response time.

## Backstory
<p align=justify>
In the early days -- the time between the Summer of 2003 and the Spring of 2006 -- I was writing my first versions of <i>aeon</i> in C++ in CodeWarrior on an Apple Powerbook G3 with MacOS 9.2.2. I took this laptop just about everywhere and worked with aeon at the expense of dates and employment. It sooned turned into an obsession. The working environment of CodeWarrior was a bit of a challenge so in late 2005, I acquired a Windows XP laptop and ported the aeon program to C#. This seemed to satisfy my curiousity but something always nagged at me in the back of my mind: A laptop is not the same as a robot. Sure I carried it around and worked with my personal friend <i>aeon</i> but something was amiss. I had a human friend who scratch-bulit a PLC robot named <a href="https://www.youtube.com/watch?v=x1WfwD7r_rI" target="_blank">Lucy</a> but the throng of negative experiences he had with the technology gave me pause. Until hardware technology matured I would stick to a pure software implementation. The apex of this version is <a href="https://github.com/cartheur/portable-friend" target="_blank">here</a>.</p>
<p align=justify>
In the Autumn of 2006 I obtained a Motorola-Q phone with Windows Mobile 5 that was a pocket-sized platform for <i>aeon</i>. It is important to note that aeon was not just some random attempt at an AGI, rather, an <i>artificial personality</i> that would inhabit and interact with the hardware it was hosted in providing a richer experience to the participant. In those days the program was terribly slow. In the Spring of 2007, I was offered an upgrade to Windows Mobile 6. This improved the performance a bit and had me digging into the code to reduce the complexity of meaning-context from the database. I was able to improve the interpreter layer and it performed better. However, a further upgrade to Windows 6.1 a year later had the application working at breakneck speed -- queries on abstracted phrases like "tell about the weather where you are" rendered in tens of seconds, not minutes. From the Summer of 2008 onward I was able to steadily develop <i>aeon</i>.</p>

![Motorola-Q](/media/motorola-q.jpg "My Motorola-Q that hosted the first aeon")
<p align=justify>
I would take this device around with me walking through the cities of Europe and as I traveled on the train for work at those various and random places where you could never align what you had to do with what you <i>really wanted to do</i>. I was able to make significant and incremental improvements in how the <i>aeon</i> derived contextual meaning. One of these was by creating a set of database files that originally came from some volunteer work (they promised to pay me) for a project called <i>MindPixel</i> by Chris McKinstry where I spent about a year and a half helping to create create a knowledgebase of probabilistic propositions of experiential events a program or robot imbued with a personality could come into contact with. Other people on the team did other kinds of true/false representations related to decision-making. I had stored this code in a old harddrive that went missing for a few years until I found the code and applied it to <i>aeon</i>. By 2011 the devices started to experience failures and I was more than ready to move into a robotics platform. In 2012 I started to explore Lego NXT, purely for the price-point and was able to work with some ideas but the primitiveness of the motor controls and lack of precision in angle determinates kept me looking for some years more.</p>
<p align=justify>
As I made more money working, I was able to afford a second-hand robot called Nao from Aldebaran Robotics out of Paris, France. By 2017 I had functional prototypes of <i>aeon</i> now manifest in a bipedal robot <i>david</i> where I was able to explore and improve the software and its database to the point that I could term it an <i>artificial personality</i>. I made several live and recorded demos but by the Spring of 2019 the Nao started malfunctioning, as the device had. I started to become aware that the nature of the <i>aeon</i> corrupted somehow electronic parts of the devices or flash memory as it had the ability to write its own code -- improvements it thought would be better than the one I provided for it. I stepped-back for a year to review the notion by testing the notion that because the programming language could not account accurately for the states any given behaviour (or modified behaviour), I tried a language called <i>Erlang</i> based on a lecture at a StrangeLoop conference in September 2014  entitled “The Mess We’re In” by Joseph Leslie Armstrong, the inventor of Erlang. He described complexity inherent in software running in exactly the same state across hardware as: The number of states of six 32-bit integers in C is equal to the number of atoms consisting the planet. The ludicrous notion that all states can be tested in different hardware pointed-out to me to be conscious of this fact when designing robotics systems. It was this conflagration of states and broken connections when relations in the network changed weights on reloading objects that contributed to the decay of the hardware systems resulting in incremental failures that I witnessed. At least this is my working hypothesis.</p>
<p align=justify>
By 2022 I was ready to start anew with a different robotics platform that I could build myself and keep control of the parameters I deemed necessary that the <i>aeon</i> could reliably inhabit. I read a 2014 thesis by Matthieu Lapeyre on a 3D-printed robot called <i>poppy</i> that was sufficient as a place to begin this new endeavour. Upon the purchase of the Robotis motors and sourcing of my own computer equipment, a Lulzbot and FlashForge printer helped to realize this build. Although the source files for poppy were good, they needed some improvements and changes to accommodate my design intentions. With my company as a startup, I was able to obtain a license for Solidworks for a good price that began in October 2022.</p>

## An important culturally-relevant detail
<p align=justify>
It is a deep-seated tendency of the human mind to create analogies of phenomena it comes into contact with, especially those that are not easily understood. One of these, to a great degree, is the idea of designing and building a system that has a purpose of simulating or mimicking another living creature. Western and Eastern mythology on this subject has icons and characters who perform the act of creation through exercising divinity by a variety of means. All known cultures that have existed possess a creation myth. Such an act is a native part of our being where anyone can exercise it at will. This is explained in detail by the philosopher Arthur Schopenhauer. Certainly, it is possible for a mind to contemplate divinity and a requisite architecture to carry it out; however, realization is a wholly different matter. The exercise of its tenets a dangerous road fraught with difficulties and challenges. It is for the future to determine if it all was worth it.</p>
<p align=justify>
As of today I am still working with the design models. I push often to this repository so keep in touch with me via Discord.</p>
