STDOUT.sync = true # DO NOT REMOVE
# Auto-generated code below aims at helping you parse
# the standard input according to the problem statement.

$R = gets.to_i # the length of the road before the gap.
$G = gets.to_i # the length of the gap.
$L = gets.to_i # the length of the landing platform.

# game loop
loop do
    $S = gets.to_i # the motorbike's speed.
    $X = gets.to_i # the position on the road of the motorbike.
    
    # Write an action using puts
    result = "WAIT"
    distance = ($R - 1) - $X   # the first section of the gap is the last segment of "road"
    STDERR.puts "Distance to Gap: " + distance.to_s + ", Width: " + $G.to_s
    gapSpeed = $G + 1 #make it over the gap
    if ($S != gapSpeed and distance > 0) then
        if ($S < gapSpeed) then
            speedUpDistance = 0
            (($S + 1)..gapSpeed).each do |i|
                speedUpDistance += i    # add the distance for each move involved in speeding up
            end
            STDERR.puts "Speed Up distance: " + speedUpDistance.to_s + ", Gap Speed: " + gapSpeed.to_s
            extraDistance = speedUpDistance - distance
            if (extraDistance % gapSpeed != 0) then # wait for the right moment to speed up
                if ($S == 0) then
                    result = "SPEED"    #move at least at speed of 1
                else
                    result = "WAIT"
                end
            else
                result = "SPEED"
            end
        else
            if (distance % $S == 0) then
                slowDownDistance = 0
                (0..$S).each do |i|
                    slowDownDistance += i
                end
                STDERR.puts "Slow Down distance: " + slowDownDistance.to_s + ", Landing Pad Length: " + $L.to_s
                if ($L < slowDownDistance) then
                    result = "SLOW"
                else
                    result = "WAIT"
                end
            else
                result = "SLOW"
            end
        end
    else
        if (distance == 0) then #at the gap
            result = "JUMP"
        elsif ($X >= $R)   #past the gap
            result = "SLOW"
        end
    end
    #if (distance >= 0) then # not yet at the gap
    #    if (distance >= $S) then # won't arrive at gap this turn
    #        if ($S <= $G) then  #not fast enough
    #            result = "SPEED"
    #        elsif (distance % $S == 0) then
    #            # we're good
    #        else    # not correctly aligned
    #            gapMiss = distance % $S
    #            moves = distance / $S
    #            if (gapMiss > moves) then   # can't go fast enough
    #                result = "SLOW"
    #            else    # must go faster!
    #                result = "SPEED"
    #            end
    #        end
    #    else
    #        result = "JUMP"
    #    end
    #elsif (distance < 0) then # made the jump
    #    result = "SLOW"
    #end
    # To debug: STDERR.puts "Debug messages..."
    
    puts result # A single line containing one of 4 keywords: SPEED, SLOW, JUMP, WAIT.
end