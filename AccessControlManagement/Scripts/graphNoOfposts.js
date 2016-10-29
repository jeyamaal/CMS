  window.onload = function () {
      var chart = new CanvasJS.Chart("chartContainer",
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
  }
