﻿namespace Core.Model.Delivery
{
    public class DeliveryInfoCreateModel
    {
        public string RecipientName { get; set; }
        public long PostDepartmentId { get; set; }
        public long PaymentTypeId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
