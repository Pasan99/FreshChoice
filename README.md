# FreshChoice - Online Supermarket

You are required using an appropriate approach to analyse, design, implement and test the software for the scenario described below:

Scenario

FreshChoice supermarket wishes to provide an online ordering, pickup and delivery facility for their customers to reduce rush within the supermarket and also to provide a convenient service. The supermarket contains variety of categories of products needed for households. There are more than 10 product categories with various items, and brands under each category which under control of different counters such as, grocery items, vegetable, meat and seafood, bakery items, dairy products, cosmetics, etc. They are taking delivery orders only within 4 km from their supermarket. The customers can also visit the supermarket and pick up the order once it is ready. When the order is made, the system will estimate and notify the approximate time needed to ready the order. The customers can be Registered customers and the Non-registered customers. All the online orders are to be payed online. Non-registered customers have to register before they make their order.
 
You are required to design a device responsive website to achieve above objectives. High-level system requirement as follows.
1.	System has two components 
  a.	Client web Application.
  b.	Web server with the store database. 
2.	The users should be able to order the items based on their preferences through a categorical search and selection facility. 
3.	Under each category the items to be updated based on the availability. The new stock should be added to the store, in each day morning which will update the available list and quantity.
4.	For each and every item, the quantity which could be prepared will be marked and if it is exceeded the available item quantity, then it should be automatically removed from the available list.
5.	The prices are fixed for each item.
6.	The customer can go to the web and login if he is a loyal customer or else register and select the categorical items from the online system.
7.	The customer can select the items with the required quantity based on the availability. After selecting all the items and the quantities the bill amount will be automatically calculated. 
8.	There are two options to obtain the order.
  a.	Home Delivery
  b.	Pick up
    -	If it is home delivery the mileage will be calculated and the Rs.50 per km will be charged.
    -	If it is pick up order, the customer has to visit the supermarket after the ready status notification and show the online confirmation receipt and take the order. 

9.	For all loyal customers, the history of purchase record should be viewed by the customer.   
10.	The Bill no and the confirmation no will be issued to the customer in the confirmation receipt.
11.	The relevant sales assistants of each categorical counter should be able to see the orders needed to be prepared by their counter according to the bill number. Each counter will be notified a deadline time to prepare the required part of the order.
12.	Once the counter has completed the part of the order, respective sales assistant will mark the status of the order part as READY. 
13.	The order manager will collect the completed parts from the counters with respective to the given orders and complete the order before sending to the dispatch counter. 
14.	The dispatch manager, again verify the ordered items with the prepared items before the delivery or pick up.
15.	The Customer can cancel the order, but the paid amount will be transfer to customers supermarket wallet for future payments. This wallet credit can be used to pay the bills.
16.	If it is a Home delivery automatically the available delivery person will be assigned with the bill number.
17.	After assigning the person, the delivery person’s Contact No. and the name will be sent to the customer as a notification.
18.	Once the order is delivered to the customer the delivery person will update the system as “DELIVERED”. 
19.	If it’s a pick up order, dispatch manager in the pick up counter has to mark the order as “DELIVERED”.
20.	The supermarket manager should be able to see the Delivery and the status of the delivery through an online system.
