<template>
    <v-container>
        <v-data-table
            v-bind:headers="headers"
            v-bind:items="shades"
            show-group-by>
            <template slot="top">
            </template>
            <template v-slot:item.layoutImageFilePath="props">
                <v-img v-bind:src="`${props.item.layoutImageFilePath}`" max-width="100"></v-img>
            </template>
        </v-data-table>
    </v-container>
</template>

<script>
export default{
    data(){
        return{
            headers: [
                {
                    text: 'Original Shade Name',
                    value: 'shadeName',
                    groupable: false
                },
                {
                    text: 'Custom Shade Name',
                    value: 'customShadeName',
                    groupable: false
                },
                {
                    text: 'Quantity',
                    value: 'quantity',
                    groupable: false
                },
                {
                    text: 'Tint Type',
                    value: 'tintType'
                },
                {
                    text: 'Layout',
                    value: 'layoutImageFilePath',
                    sortable: false,
                    filterable: false,
                    groupable: false
                },
            ],
            shades: []
        }
    },
    props: ['customerOrderDetails'],
    beforeMount(){
        const that = this;
        this.$axios({
            method: 'get',
            crossdomain: false,
            url : `api/customer-order/${that.$route.params.id}/shades`
        }).then(res => {
            that.shades = res.data.map(s => {
                return {
                    id: s.Id,
                    customerOrderId: s.CustomerOrderId,
                    shadeId: s.ShadeId,
                    quantity: s.Quantity,
                    shadeName: s.ShadeName,
                    customShadeName: s.CustomShadeName,
                    layoutImageFilePath: s.LayoutImageFilePath,
                    tintType: s.TintType
                }
            });
        });
    }
}
</script>