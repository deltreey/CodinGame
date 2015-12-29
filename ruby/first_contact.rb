$terrain = Array.new
$landingAreas = Array.new

STDOUT.sync = true # DO NOT REMOVE
# Auto-generated code below aims at helping you parse
# the standard input according to the problem statement.

$N = gets.to_i # the number of points used to draw the surface of Mars.
$N.times do
    # LAND_X: X coordinate of a surface point. (0 to 6999)
    # LAND_Y: Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
    $LAND_X, $LAND_Y = gets.split(" ").collect {|x| x.to_i}
    $terrain.push({ "x" => $LAND_X, "y" => $LAND_Y })
end

def findTargetArea()
    $terrain.each_with_index do |item, i|
        if (i > 0 && item["y"] == $terrain[i - 1]["y"]) then
            $landingAreas.push(i)
        end
    end
end

findTargetArea()

# game loop
loop do
    # HS: the horizontal speed (in m/s), can be negative.
    # VS: the vertical speed (in m/s), can be negative.
    # F: the quantity of remaining fuel in liters.
    # R: the rotation angle in degrees (-90 to 90).
    # P: the thrust power (0 to 4).
    $X, $Y, $HS, $VS, $F, $R, $P = gets.split(" ").collect {|x| x.to_i}
    
    resultAngle = 0
    resultThrust = 4
    
    #X location fixing
    if ($X > $terrain[$landingAreas[0]]["x"]) then
        resultAngle = 90
    elsif ($X < $terrain[$landingAreas[0] - 1]["x"]) then
        resultAngle = -90
    else
        resultAngle = 0
    end
    
    if ($Y > $terrain[$landingAreas[0]]["y"]) then
        if ($VS < -35) then
            resultTHrust = 4
        else
            resultThrust = 3
        end
    elsif ($Y <= $terrain[$landingAreas[0]]["y"]) then
        if (resultAngle == 90) then
            resultAngle = 45
        elsif (resultAngle == -90) then
            resultAngle == -45
        else
            resultAngle = 0
        end
    end
    
    # To debug: STDERR.puts "Debug messages..."
    
    puts resultAngle.to_s + " " + resultThrust.to_s # R P. R is the desired rotation angle. P is the desired thrust power.
end