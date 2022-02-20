INCLUDE GlobalVariables.ink

//Asking Silk for Help Alternate
//Draft 1 | Text Updated 2/18 | Ink File Updated 2/18
//This version of Chapter 5 is for use if we are unable to implement the duel with Silk in full. It should cover the main story beats and keep things moving forward.
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR usualSpot = "Tower's Crest Viewpoint"
VAR sceneFinished = false
-> Start

===Start===
#tab: Party #s1lverSun
So uh.
The Usual Spot?
Where is the Usual Spot?
{ //Conditional Split
    - silkRelationship + willowRelationship >= 2: //You've been nice
        -> Nice
    - else: //You've been rude
        -> Rude
}
= Nice
#silkenscraps
It's past the town gate, then south of town.
#s1lverSun
Wow, that was surprisingly helpful of you.
#silkenscraps
Look, you're a total stranger and a mssive loser.
#you
There it is.
#silkenscraps
But you've done right by me and Willow.
So, thank you.
Can I tell you something?
#s1lverSun
I've been a captive audience up to now, what the hell, go for it.
#silkenscraps
Seeing you on that account almost makes it feel like Sunny's still there.
Just a message away.
I miss him so much.
Anyway. Willow will be waiting for you.
Don't be late.
~ scene05Finished = true
~sceneFinished = true
-> END
= Rude
#silkenscraps
Wouldn't you like to know.
#s1lverSun
Yes! That's why I'm asking!
You want me to pretend to be Sunny, you're going to need to help me a little here.
#silkenscraps
It's past the portal south of town.
#s1lverSun
Finally, thank you!
#silkenscraps
I can't wait for today to end.
I never want to deal with you again.
You're an insult to Sunny's memory in every way.
#s1lverSun
I didn't sign up to deal with all his baggage.
#silkenscraps
Willow will be waiting for you.
Don't disappoint them.
A\*****e.
~sceneFinished = true
~ scene05Finished = true
-> END