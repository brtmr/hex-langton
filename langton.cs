
DIRECTION_LEFT  = -1
DIRECTION_RIGHT =  1

canvas_config = 
    width:700
    height:500

hex =
    width: 7
    height: 2.0/Math.sqrt(3) * 7

window.onload = () ->
    canvas = document.getElementById("langton")
    tryit = document.getElementById("tryit")
    context = canvas.getContext('2d');

    # the amount of hexagons we can fit along the x-axis:
    j_max = (canvas_config.width/hex.width) - 2
    # the amount of hexagons we can fit along the y-axis:
    i_max = (canvas_config.height/(3.0/4.0 * hex.height)) - 1
    # draw a single hexagon around the centerpoint x,y


    clear = () ->
        canvas.width = canvas.width

    drawSingleHex = (i,j) ->
        cos = mk_coordinates(i,j)
        x = cos[0]
        y = cos[1]
        if black[i][j]
            cos = mk_coordinates(i,j)
            context.beginPath()
            context.moveTo(x-0.5*hex.width, y-0.25*hex.height)
            context.lineTo(x              , y- 0.5*hex.height)
            context.lineTo(x+0.5*hex.width, y-0.25*hex.height)
            context.lineTo(x+0.5*hex.width, y+0.25*hex.height)
            context.lineTo(x              , y+ 0.5*hex.height)
            context.lineTo(x-0.5*hex.width, y+0.25*hex.height)
            context.lineTo(x-0.5*hex.width, y-0.25*hex.height)

            context.lineWidth = 1
            context.strokeStyle = '#aaa'
            context.stroke()
            context.fillStyle = 'black'
            context.fill()

    #turns a row and column number into a centerpoint
    mk_coordinates = (i,j) ->
        #on even rows
        if (i%2==0)
            x=(hex.width/2.0) + j*hex.width
        else
            x=(j+1)*hex.width
        y=(hex.height/2.0) + i*(hex.height*(3/4))
        return [x,y]

    ant =
        direction : 0
        i         : Math.floor(i_max/2)
        j         : Math.floor(j_max/2)

    draw_ant = () ->
        cos = mk_coordinates(ant.i,ant.j)
        context.beginPath()
        context.arc(cos[0], cos[1], 2, 0, 2 * Math.PI, false);
        context.fillStyle = 'red'
        context.fill()

    move = (dir) ->
        ant.direction += dir
        if ant.direction > 5 
            ant.direction = 0
        if ant.direction < 0
            ant.direction = 5

    draw_board = () ->
        for i in [0..i_max]
            for j in [0..j_max]
                drawSingleHex(i,j)

    # make a 2dimensional array to store the colors of a block
    black = []
    for i in [0..i_max]
        row = []
        for j in [0..j_max]
            row[j]=0
        black[i] = row

    step = () -> 
        if ant.direction == 1
            ant.j = ant.j + 1
            return
        if ant.direction == 4
            ant.j = ant.j - 1
            return
        # are we in an even row?
        if ant.i%2==0
            #top right
            if ant.direction == 0
                ant.i = ant.i - 1
            #bottom right
            if ant.direction == 2
                ant.i = ant.i + 1
            #bottom left
            if ant.direction == 3
                ant.i = ant.i + 1
                ant.j = ant.j - 1
            #top left
            if ant.direction == 5
                ant.i = ant.i - 1
                ant.j = ant.j - 1
        else
            #top right
            if ant.direction == 0
                ant.i = ant.i - 1
                ant.j = ant.j + 1
            #bottom right
            if ant.direction == 2
                ant.i = ant.i + 1
                ant.j = ant.j + 1
            #bottom left
            if ant.direction == 3
                ant.i = ant.i + 1
            #top left
            if ant.direction == 5
                ant.i = ant.i - 1

    walk = () -> 
        if black[ant.i][ant.j]
            move(DIRECTION_LEFT)
            black[ant.i][ant.j] = 0
        else
            move(DIRECTION_RIGHT)
            black[ant.i][ant.j] = 1
        step()

    run = () -> 
        walk()
        if ant.j > j_max || ant.j<0 || ant.i > i.max || ant.i < 0
            return
        clear()
        draw_board()
        draw_ant()
        setTimeout(run,10)
    tryit.addEventListener('click',run)
