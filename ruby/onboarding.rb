STDOUT.sync = true # DO NOT REMOVE
# The code below will read all the game information for you.
# On each game turn, information will be available on the standard input, you will be sent:
# -> the total number of visible enemies
# -> for each enemy, its name and distance from you
# The system will wait for you to write an enemy name on the standard output.
# Once you have designated a target:
# -> the cannon will shoot
# -> the enemies will move
# -> new info will be available for you to read on the standard input.


# game loop
loop do
    $count = gets.to_i # The number of current enemy ships within range
    nearest = "None"
    lowestDistance = 999
    $count.times do
        # enemy: The name of this enemy
        # dist: The distance to your cannon of this enemy
        $enemy, $dist = gets.split(" ")
        $dist = $dist.to_i
        #STDERR.puts "current distance: " + $dist.to_s + ", current enemy: " + $enemy.to_s
        #STDERR.puts "distance: " + lowestDistance.to_s + ", enemy: " + nearest.to_s
        if ($dist < lowestDistance) then
            lowestDistance = $dist
            nearest = $enemy
        end
    end
    
    # Write an action using puts
    # To debug: STDERR.puts "Debug messages..."
    #STDERR.puts "Nearest " + nearest
    
    puts nearest # The name of the most threatening enemy (HotDroid is just one example)
end