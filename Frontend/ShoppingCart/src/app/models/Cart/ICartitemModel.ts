/**
 * Data model that holds information related to a cart item.
 */
export interface ICartitemModel {
  /// <summary>
  /// Autogenerated ID that uniquely identifies each cart item.
  /// </summary>
  /// <example>1</example>
  CartID: number;
  /// <summary>
  /// Uniquely identifies each cart item with a unique order id
  /// </summary>
  /// <example>1</example>
  OrderID: number;
  /// <summary>
  /// Uniquely identifies each order with the user.
  /// </summary>
  /// <example>1</example>
  UserID: number;
  /// <summary>
  /// Username of the user.
  /// </summary>
  /// <example>karthik</example>
  Username: string;
  /// <summary>
  /// Represents a unique product added to the cart
  /// </summary>
  /// <example>1</example>
  ProductID: number;
  /// <summary>
  /// Product name of the cart item
  /// </summary>
  /// <example>Branded Foods</example>
  Productname: string;
  /// <summary>
  /// Defines the price of this product
  /// </summary>
  /// <example>100</example>
  ProductPrice: number;
}
