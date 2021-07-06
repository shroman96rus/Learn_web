
function getInfo(id) {

    $.post("/Statistic/Month", { Month: id })
        .done(function (data) {

            
           
            $("#result").html(data);
            console.log(data);
        });

    $.post("/Statistic/GrafMonth", { Month: id })
        .done(function (data) {

            console.log(data);

            if (window.myChart instanceof Chart)
            {
                window.myChart.destroy();
            }



            var count = data.length;

            var arrColl = [];

            var arrDate = [];

            for (var i = 0; i < count; i++) {
                
                arrDate.push(data[i].summWork);
            }

            for (var i = 0; i < count; i++) {
                arrColl.push(data[i].summDate);
            }
          
            console.log(arrDate);
            console.log(arrDate);
            
            var ctx = document.getElementById('myChart').getContext('2d');

            const currentMonth = document.getElementById('statistic-grafic-month').value;

            console.log(currentMonth);

            //var date = document.getElementById('#changeMonth').innerHTML;

            
            window.myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: arrColl,
                    datasets: [{
                        label: 'График заработка за месяц ' + id,
                        
                        backgroundColor: [
                            '#00FF00',

                        ],
                        borderColor: [

                            '#FF00FF'
                        ],
                        data: arrDate,
                    }],
                },
                
            });

        });

   
}





