-> Knot_One
//This test ink file is meant purely to test how Ink's internal flow might interact with a given dialogue system. It uses multiple knots that are connected to each other, with some conditional checks.
=== Knot_One ===
This is text.
This is a second line of text.
//This is a comment
#This is a tag
This is a third line of text with choices.
*   Choices?
    Yeah choices.
*   Choices.
    Maybe choices.
- And so it goes.
-> Knot_Two

=== Knot_Two ===
This is a second Knot.
*   [Continue to Third Knot]-> Knot_Three

=== Knot_Three ===
You are now in Knot_Three.
*   [This should return you to Knot_Three and disappear.] -> Knot_Three
+   [This should let you return to Knot_Three repeatedly.] -> Knot_Three
+   {Knot_Four} [This should take you to Knot_Five if you've been to Knot_Four.] -> Knot_Five
+   [This should take you to Knot_Four.] -> Knot_Four

=== Knot_Four ===
You've made it to Knot_Four.
+   [Go back to Knot_Three.] -> Knot_Three

=== Knot_Five ===
This is Knot_Five, good bye.
    -> END
