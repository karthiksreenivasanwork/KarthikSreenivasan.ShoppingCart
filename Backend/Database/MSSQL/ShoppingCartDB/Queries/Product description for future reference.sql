/*
Product description for reference
	* Pepsi			-	Pepsi is a carbonated soft drink manufactured by PepsiCo	0.png
	* Fortune Oil	-   Fortune Sunflower oil or Fortune Sunlite Oil is light, healthy and perfectly easy to digest this makes it the best oil for cooking.	1.png
	* Vim			-	Vim Dishwash Bar is the No.1 dishwash brand in India · It provides a pleasant cleaning experience with its refreshing lemon fragrance during dishwash.	2.png
	* Banana		-	A banana is an elongated, edible fruit – botanically a berry – produced by several kinds of large herbaceous flowering plants in the genus Musa.	3.png
*/

update T_Products
set ProductDescription = 'A banana is an elongated, edible fruit – botanically a berry – produced by several kinds of large herbaceous flowering plants in the genus Musa.'
where ProductID = 31

select * from T_Products

