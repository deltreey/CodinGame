# Auto-generated code below aims at helping you parse
# the standard input according to the problem statement.

@n = gets.to_i # Number of elements which make up the association table.
@q = gets.to_i # Number Q of file names to be analyzed.
@mimes = {};
@n.times do
    # ext: file extension
    # mt: MIME type.
    ext, mt = gets.split(' ')
    # STDERR.puts 'extension: ' + ext.upcase
    # STDERR.puts 'mime: ' + mt
    @mimes[ext.upcase] = mt
end

@q.times do
    fname = gets.chomp # One file name per line.
    STDERR.puts 'file: ' + fname
    segments = fname.split('.', -1)
    if (segments.count > 1)
        extension = segments.last.upcase
        STDERR.puts 'extension: ' + extension
        if (@mimes.include?(extension))
            STDERR.puts 'mime: ' + @mimes[extension]
            puts @mimes[extension]
        else
            puts 'UNKNOWN'
        end
    else
        puts 'UNKNOWN'
    end
end