INCLUDE GlobalVariables.ink

//Last Day with Willow
//Draft 1.1 | Text Updated 2/9 | Ink File Updated 2/13
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR willowSuspicion = 0
    //Same as for willowRelationship
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#tab: Willow #w1ll0w_w1sp
Hey Sunny, come over here!
I got somethign for you.

//Willow gives Silver a super rare item

#s1lverSun
What's this?
#w1ll0w_w1sp
I remember a while back you mentioned wanting one.
Before you left and all.
It's the last day of the game, so prices are dirt cheap.
What do you think?

+   [Love It]
    ~ willowRelationship += 2
    #s1lverSun
    Wow, that's really sweet of you.
    I love it.
    #w1ll0w_w1sp
    I'm glad! Sorry it won't be all that much use to you now.
    #s1lverSun
    I'll cherish it for as long as this game is running.
    #w1ll0w_w1sp
    Hah, a\*****e.
+   [Hate It]
    ~ willowRelationship -= 2
    #s1lverSun
    Wow, uh.
    #w1ll0w_w1sp
    What?
    #s1lverSun
    What am I supposed to do with it now?
    #w1ll0w_w1sp
    I just thought it might make you happy.
    No need to be an a\*****e about it.
    // Awkward Pause

-
#w1ll0w_w1sp
Look, Sunny, I need to ask.
Why did you leave?
#s1lverSun
I told you, no reason in particular.
#w1ll0w_w1sp
It wasn't because of me, was it?
I know you said you weren't feeling well back then.
But I've been worried that it was something I did, or something I said.
+   [Comfort]
    ~ willowRelationship += 2
    #s1lverSun
    No, not at all Willow.
    Things just came up that I couldn't stop.
    It's not your fault at all.
    #w1ll0w_w1sp
    That's a relief.
+   [Say Nothing]
    ~ willowRelationship -= 2
    #w1ll0w_w1sp
    I see...
    //Awkward Pause

-
#w1ll0w_w1sp
It's not a bad view, huh?
The graphics aren't amazing, but I'm going to miss this.
Is it strange to feel that way?
After today, we'll never be able to come back here.
+   [Support]
    ~ willowRelationship +=2
    #s1lverSun
    I don't think it's that strange.
    I feel that way too.
+   [Say Nothing]

-
#w1ll0w_w1sp
I can't help thinking about all the things we've done in the game.
And the things that could have been if the game wasn't shutting down.
I just don't know, Sunny.
How do I say goodbye to a place?
And how do I say goodbye to a friend?
+   [Speak Up]
    -> Speak
+   [Say Nothing]
-
#w1ll0w_w1sp
Silk messaged me earlier, mentioned staying in touch, maybe finding another game.
I'd like that.
But even then, it won't be the same.
I won't be the same w1ll0w_w1sp.
You won't be the same s1lverSun.
We'll never have the chance to be who we are right here, right now in this game.
+   [Speak Up]
    -> Speak
+   [Say Nothing]
-
#w1ll0w_w1sp
I guess what I'm saying is, I missed you.
I missed the things we used to do.
#ingame: Dialogue #[SERVER]
GAME WILL BE GOING OFFLINE PERMANENTLY IN 5 MINUTES
#tab: Willow #w1ll0w_w1sp
Looks like we're running out of time here.
+   [Speak Up]
    -> Speak
+   [Say Nothing]
-
#w1ll0w_w1sp
I wish we could have had even more time together, here in this game.
I'm glad I got the chance to play with you one last time.
So thanks for coming back and sticking around.
I hope we can stay in touch in the future.
Goodbye s1lverSun.
#[SERVER]
w1ll0w_w1sp IS NOW OFFLINE
~ sceneFinished = true
-> END

= Speak
#s1lverSun
Willow, there's something I need to tell you.
#w1ll0w_w1sp
What's up?
+   [Tell the Truth]
    -> Truth
+   [Say Goodbye]
    -> Goodbye

= Truth
#s1lverSun
I'm not s1lverSun.
I bought a hacked account online to catch the end of the game.
And I tried to trick you so I wouldn't get caught.
But I had a really good time playing with you today.
And I'm sorry for not telling you sooner.
#w1ll0w_w1sp
I see.
{   - willowSuspicion > 2:
    <> I kind of knew for a while.
    - else:
    <> I kind of had a feeling.
}
But I was just happy to be here one last time.
Playing the game with Sunny.
Do you know what happened to him?
#s1lverSun
Silk told me he died a few months back.
I'm sorry.
#w1ll0w_w1sp
I never got the chance to say goodbye.
I think I need some time to myself.
Thank you for telling me the truth.
#[SERVER]
w1ll0w_w1sp IS NOW OFFLINE

~ sceneFinished = true
-> END

= Goodbye
#s1lverSun
I had a really good time playing with you today.
And all the times before as well.
I feel like the things we did today were only a small part of all the fun we've had together.
And I hope that you find even more joy in the future.
Goodbye Willow.
It's been great getting to know you.
#w1ll0w_w1sp
And yuou as well.
I wish we could have had even more time together.
And I wish you hadn't dropped off the face of the earth.
So thanks for coming back and sticking around.
Goodbye s1lverSun
#[SERVER]
w1ll0w_w1sp IS NOW OFFLINE


~ sceneFinished = true
~ scene06Finished = true
-> END