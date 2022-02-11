//Willow Reaches Out
//Draft 1 | Text Updated 2/7 | Ink File Updated 2/10
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
#willow
Yoooooo, is that you Sunny?
I haven't seen you online in ages! How have you been?
+   [Play it off]
    ~ willowRelationship++
    #silver
    I've been good, you?
    #willow
    Same old, same old.
+   [Express Confusion]
    ~ willowRelationship--
    ~ willowSuspicion++
    #silver
    Who are you again?
    #willow
    Ouch man, it hasn't been that long I hope.
    It's me, Willow. We played every Friday night?
    #silver
    Oh, right, sorry. It's been a while.
-
#willow
What hapened anyway?
You kind of dropped the game without any warning.
I was getting a little worried.
#silver
Stuff came up, life got in the way, you know how it goes.
#willow
Sure do! Well, you're here now.
I'm happy to see you again, Sunny.
Hey.
Last day of the game, let's party up for the rest of the day and end it properly.
What do you say?
+   [Agree]
    ~ willowRelationship++
    #silver
    Sure.
    #willow
    Alright! Meet me at {meetingPlace} when you're ready and we'll get this party started!
+   [Deflect]
    ~ willowRelationship--
    ~ willowSuspicion++
    #silver
    I don't know, I kind of wanted to do some solo content.
    #willow
    Yeah right, on the last day of the MMO?
    You can't be serious.
    I don't see you for months and you come back on the last day of the game only to say you want to play solo.
    Meet me at {meetingPlace}, there's no way I'm letting you spend the day alone.
-
~ sceneFinished = true
-> END