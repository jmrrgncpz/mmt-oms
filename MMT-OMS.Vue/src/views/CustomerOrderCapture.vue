<template>
    <v-container>
        <v-toolbar color="white" flat>
            <v-tabs v-model="tab" class="no-print">
                <v-tab 
                    v-for="tabItem in tabItems"
                    v-bind:key="tabItem.path"
                    v-bind:to="{ name : tabItem.pathName }">
                    {{ tabItem.name }}
                </v-tab>
            </v-tabs>
            <v-spacer></v-spacer>
            <v-btn icon v-on:click="refreshOrder">
                <v-icon>mdi-refresh</v-icon>
            </v-btn>
        </v-toolbar>
        
        <router-view v-bind:customerOrderDetails="customerOrderDetails"></router-view>
    </v-container>
</template>

<script>
export default{
    data(){
        return{
            tab : '',
            tabItems : [
                {
                    pathName : 'customer-order-details',
                    name : 'Details'
                },
                {
                    pathName : 'customer-order-shades',
                    name : 'Shades'
                },
                {
                    pathName : 'customer-order-payments',
                    name : 'Payments'
                }
            ],
            customerOrderDetails: null,
        }
    },
    methods: {
        refreshOrder(){  
            this.loadOrder();
        },
        loadOrder(){
            const that = this;
            this.$axios({
                method: 'get',
                crossdomain: false,
                url : `api/customer-order/${ that.$route.params.id }`
            }).then(({data}) => {
                that.customerOrderDetails = {
                    amount: data.Amount,
                    brandName: data.BrandName,
                    bundleCode: data.BundleCode,
                    deliveryAddress: data.DeliveryAddress,
                    clientFacebookName: data.ClientFacebookName,
                    clientName: data.ClientName,
                    id: data.Id,
                    phoneNumber: data.PhoneNumber,
                    uniqueCode: data.UniqueCode,
                    orderStatusId: data.OrderStatusId
                };
            })
        }
    },
    beforeMount(){
        this.loadOrder();
    }
}
</script>