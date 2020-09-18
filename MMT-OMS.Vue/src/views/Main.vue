<template>
    <div>
        <v-navigation-drawer permanent dark app class="no-print">
            <v-list dense nav>
                <v-list-item two-line>
                    <v-list-item-content>
                        <v-list-item-title>Maureen Meriel Tints</v-list-item-title>
                        <v-list-item-subtitle>Order Management System</v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item>

                <v-divider></v-divider>
                <v-list-item v-for="item in items" v-bind:key="item.title" v-on:click="goTo(item.routeName)">
                        <v-list-item-icon>
                            <v-icon>{{ item.icon }}</v-icon>
                        </v-list-item-icon>

                        <v-list-item-content>
                            <v-list-item-title>{{ item.title }}</v-list-item-title>
                        </v-list-item-content>
                </v-list-item>
            </v-list>
        </v-navigation-drawer>
        <v-main id="main">
            <router-view></router-view>
        </v-main>   
    </div>
</template>


<script>
  export default {
    data () {
      return {
        drawer: true,
        items: [
          { title: 'Customer Orders', icon: 'mdi-format-list-bulleted', routeName : "customer-orders" },
          { title: 'Print Layouts', icon: 'mdi-printer', routeName: "print-layouts" },
          { title: 'Logout', icon: 'mdi-logout', routeName: "logout"}
        ]
      }
    },
    methods : {
        goTo(routeName){
            this.$router.push({ name : routeName });
        }
    },
    beforeMount(){}
  }
</script>

<style>
    @media screen {
        .no-screen{
            display: none !important;
        }
    }

    @media print{
        #main{
            padding: 0 !important;
        }

        .no-print {
            display: none !important;
        }
    }
</style>