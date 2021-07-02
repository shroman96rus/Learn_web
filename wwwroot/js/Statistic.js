
function getInfo(id) {

    $.post("/Statistic/Month", { Month: id })
        .done(function (data) {

            
            //console.log(count);

            //var result_str = "";
            //for (var i = 0; i < count; i++) {
            //    console.log(data[i]);
            //    result_str = result_str + " " + data[i];
            //}
            $("#result").html(data);
            console.log(data);
        });

    $.post("/Statistic/GrafMonth", { Month: id })
        .done(function (data) {

            console.log(data);

            var count = data.length;

            var arr1 = [];

            var arrColl = [];

            var arrDate = [];

            arrDate[0] = data[0].costOfWork;
            
            for (var i = 1; i < count; i++) {
                if (data[i - 1].dateOrder == data[i])
                {
                   arrDate.push(data[i].costOfWork + data[i-1].costOfWork);
                }
                else
                {
                    arrDate.push(data[i].costOfWork);
                }
                
            }

          
            console.log(arrDate);
            
            

            var ctx = document.getElementById('myChart').getContext('2d');

            var date = document.getElementById('#changeMonth').innerHTML;

            if (window.myChart instanceof Chart)
            {
                window.myChart.destroy();
            }

            console.log(date);

            window.myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: '',
                    datasets: [{
                        label: 'График заработка за месяц',
                        
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





