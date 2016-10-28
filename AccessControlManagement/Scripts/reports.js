//File - reports.js
//Created by Jeyamaal
//Date-28/10/2016


//You can call only one window.onload() function and add other functions inside window.onload()
window.onload = function () {

      var chart = new CanvasJS.Chart("noOfPostschartContainer",
      {
          title: {
              text: "Monthly Posts"
          },
          data: [
        {
            type: "area",
            dataPoints: [//array

            { x: new Date(2012, 00, 1), y: 6 },
            { x: new Date(2012, 01, 1), y: 3 },
            { x: new Date(2012, 02, 1), y: 3 },
            { x: new Date(2012, 03, 1), y: 2 }
           
            ]
        }
          ]
      });

      chart.render();

    var chart = new CanvasJS.Chart("monthchartContainer",
    {
        theme: "theme2",
        title: {
            text: "Income - per month"
        },
        animationEnabled: true,
        axisX: {
            valueFormatString: "MMM",
            interval: 1,
            intervalType: "month"

        },
        axisY: {
            includeZero: false

        },
        data: [
        {
            type: "line",
            //lineThickness: 3,        
            dataPoints: [
            { x: new Date(2012, 00, 1), y: 450 },
            { x: new Date(2012, 01, 1), y: 414 },
            { x: new Date(2012, 02, 1), y: 520, indexLabel: "highest", markerColor: "red", markerType: "triangle" },
            { x: new Date(2012, 03, 1), y: 460 },
            { x: new Date(2012, 04, 1), y: 450 },
            { x: new Date(2012, 05, 1), y: 500 },
            { x: new Date(2012, 06, 1), y: 480 },
            { x: new Date(2012, 07, 1), y: 480 },
            { x: new Date(2012, 08, 1), y: 410, indexLabel: "lowest", markerColor: "DarkSlateGrey", markerType: "cross" },
            { x: new Date(2012, 09, 1), y: 500 },
            { x: new Date(2012, 10, 1), y: 480 },
            { x: new Date(2012, 11, 1), y: 510 }

            ]
        }


        ]
    });

      chart.render();
  }


