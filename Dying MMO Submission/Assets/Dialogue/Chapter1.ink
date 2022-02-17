INCLUDE GlobalVariables.ink

//Willow Reaches Out
//Draft 1 | Text Updated 2/7 | Ink File Updated 2/13
VAR willowRelationship = 0
    //Max = 2
    //Min = -2
VAR willowSuspicion = 0
    //Max = 2
    //Min = 0
VAR meetingPlace = "The Rock"
VAR sceneFinished = false
-> Start

===Start===
#tab: Willow #w1ll0w_w1sp
Yoooooo, is that you Sunny?
I haven't seen you online in ages! How have you been?
+   [Play it off]
    ~ willowRelationship++
    #s1lverSun
    I've been good, you?
    #w1ll0w_w1sp
    Same old, same old.
+   [Express Confusion]
    ~ willowRelationship--
    ~ willowSuspicion++
    #s1lverSun
    Who are you again?
    #w1ll0w_w1sp
    Ouch man, it hasn't been that long I hope.
    It's me, Willow. We played every Friday night?
    #s1lverSun
    Oh, right, sorry. It's been a while.
-
#w1ll0w_w1sp
What hapened anyway?
You kind of dropped the game without any warning.
I was getting a little worried.
#s1lverSun
Stuff came up, life got in the way, you know how it goes.
#w1ll0w_w1sp
Sure do! Well, you're here now.
I'm happy to see you again, Sunny.
Hey.
Last day of the game, let's party up for the rest of the day and end it properly.
What do you say?
+   [Agree]
    ~ willowRelationship++
    #s1lverSun
    Sure.
    #w1ll0w_w1sp
    Alright! Meet me at {meetingPlace} when you're ready and we'll get this party started!
+   [Deflect]
    ~ willowRelationship--
    ~ willowSuspicion++
    #s1lverSun
    I don't know, I kind of wanted to do some solo content.
    #w1ll0w_w1sp
    Yeah right, on the last day of the MMO?
    You can't be serious.
    I don't see you for months and you come back on the last day of the game only to say you want to play solo.
    Meet me at {meetingPlace}, there's no way I'm letting you spend the day alone.
-
~ sceneFinished = true
~ scene01Finished = true
-> END