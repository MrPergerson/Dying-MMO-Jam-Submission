INCLUDE GlobalVariables.ink

//Asking Silk for Help
//Draft 1 | Text Updated 2/9 | Ink File Updated 2/13
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR usualSpot = "Tower's Crest Viewpoint"
VAR duelAccepted = false
VAR sceneFinished = false
-> Start

===Start===
//TEAM CHAT OR SILK CHAT
-> Main

//DEBUG SECTION, REMOVE LATER
    //-> Debug
    = Debug
    #s1lverSun
    This is a debug question.
    Have you been nice to me and Willow so far?
    +   [Yes]
        ~ willowRelationship += 50
        ~ silkRelationship += 50
        #s1lverSun
        Yeah, I've been trying to be friendly to you weirdos.
        #silkenscraps
        Cool. Back to the story then.
        -> Main
    +   [No]
        ~ willowRelationship -= 50
        ~ silkRelationship -= 50
        #s1lverSun
        No, you two are weird.
        #silkenscraps
        Rude, but okay. Back to the story then.
        -> Main
        
= Main
#tab: Party #s1lverSun
So uh.
The Usual Spot?
Where is the Usual Spot?
#silkenscraps
Wouldn't you like to know?
#s1lverSun
Yes, that's why I'm asking!
You want me to pretend to be Sunny, you're going to need to help me here.
The  Usual Spot doesn't sound like something that someone just forgets.
#silkenscraps
Fight me.
#s1lverSun
Oh I'd love to.
#[SERVER]
SILKENSCRAPS CHALLENGES YOU TO A DUEL
#s1lverSun
Oh.
{ //Conditional Split
    - silkRelationship + willowRelationship >= 2: //You've been nice
        -> Nice
    - else: //You've been rude
        -> Rude
}
= Nice
#silkenscraps
Sunny and I used to duel every now and then for fun.
Will you let me fight his account one more time?
Fight me, and I'll tell you where to go.
    -> Cont
= Rude
#silkenscraps
You've been a pain in the a\*s all day, and I want to blow off some steam.
Fight me, and I might tell you where to go.
    -> Cont

= Cont
#s1lverSun
Not giving me much of a choice here, are you.
+   [Agree to Fight]
    -> Fight
+   [Refuse to Fight]
    -> Refused

= Fight
~ silkRelationship += 10
~ duelAccepted = true
#s1lverSun
Alright, you're on.
#silkenscraps
I know you're new to the game, but try to put up at least a little fight.
~ sceneFinished = true
-> END

//Silk should talk during the fight, but it might be best to control that through Unity rather than Ink. I think it would be super cool to have text lines be responsive to things the player is doing, but implementation? :person_shrugging:
// For now, here are some lines to play in order.
//
//#silkenscraps
//Hah, nice dodge!
//I hate that ability.
//Come at me, Sunny!
//Your gear's fallen behind buddy.
//Nice try!
//That's what you get for leaving us behind!
//That's what you get for not saying goodbye!
//
//To control the outcome, we may need to divide this script into separate ones. For now, here's a debug choice.

= Refused
~ silkRelationship -= 10
#s1lverSun
Yeah, no, I'm not accepting that.
#silkenscraps
A\*****e
You're an insult to Sunny's memory in every way.
#s1lverSun
Look, I didn't sign up to deal with all his baggage.
And I didn't ask to be harassed by you all day.
#silkenscraps
Well now you're on your own.
Willow will be waiting for you.
Don't disappoint them.
#[SERVER]
SILKENSCRAPS HAS LEFT THE PARTY
~ sceneFinished = true
~ scene05Finished = true
-> END