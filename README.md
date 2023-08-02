# Grow A Spine Unity Demo

<h2>Objective</h2>
<p>
Grow A Spine is the PoC of a simulation of a rigid body striving 
to reach its maximum height potential. The 'Spine' is represented 
as a series of interconnected rigid bodies, with the last segment 
(in blue) acting as a fixed anchor. The red segment is subjected 
to a force vector, random in both magnitude and direction, equivalent 
to the segment's weight applied at its center of mass. Whenever the 
red segment attains a new personal height record, the model is 
adjusted to incorporate a percentage of the force vector that 
propelled it to the new position for the next iteration. This 
process continues until either no new record is set after 10,000 
cycles, or the proportion of the recycled force vector reaches 100%.
</p>
<h2>How To Play</h2>
<p>
Spacebar - Pauses and unpauses cycles <br>
R - Resets game to beginnign <br>
ESC - Quits game <br>
WASD/Arrows - move camera <br>
Mouse - rotate camera <br>
Right Click and Move Mouse - pan camera <br>
Scrollwheel - Zoom <br>
</p>
